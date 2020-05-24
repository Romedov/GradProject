using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.Models.Interfaces;
using Kassa.OtherComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Kassa.Models
{
    public class CashRegisterContext : DbContext
    {
        private static string _path = "Shop.db";
        private static Shift _currentShift;

        #region Public constructors
        public CashRegisterContext() : base()
        {
            this.CanConnect = (this.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists() && this.Database.CanConnect();
        }
        public CashRegisterContext(string path) : base()
        {
            _path = path;
        }
        #endregion
        #region Public events
        #endregion
        #region Public props
        public static User CurrentUser { get; private set; }
        public static Shift CurrentShift 
        {
            get => _currentShift;
            set
            {
                _currentShift = value ?? new Shift();
            }
        }
        public bool CanConnect { get; }
        public virtual DbSet<Item> Items { get; private set; }
        public virtual DbSet<ReceiptItem> ReceiptItems { get; private set; }
        public virtual DbSet<Shift> Shifts { get; private set; }
        public virtual DbSet<User> Users { get; private set; }
        public virtual DbSet<Receipt> Receipts { get; private set; }
        public virtual DbSet<Supply> Supplies { get; private set; }
        public virtual DbSet<Disposal> Disposals { get; private set; }
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new MyLoggerProvider());    // указываем наш провайдер логгирования
        });
        #endregion
        #region Protected methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseSqlite($"Data Source={_path}");
                optionsBuilder.UseLoggerFactory(MyLoggerFactory);
                //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }   
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Property);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Supply>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID).ValueGeneratedOnAdd();
                entity.Property(e => e.ShiftID).IsRequired();
                entity.Property(e => e.ItemID).IsRequired();
                entity.Property(e => e.Basis).IsRequired();
                entity.Property(e => e.Price).HasConversion<double>().IsRequired();
                entity.Property(e => e.Quantity).IsRequired();

                entity.HasOne(s => s.Shift)
                        .WithMany(sh => sh.Supplies)
                        .HasForeignKey(s => s.ShiftID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                entity.HasOne(s => s.Item)
                        .WithMany(i => i.Supplies)
                        .HasForeignKey(s => s.ItemID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
            });

            modelBuilder.Entity<Disposal>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID).ValueGeneratedOnAdd();
                entity.Property(e => e.ShiftID).IsRequired();
                entity.Property(e => e.ItemID).IsRequired();
                entity.Property(e => e.Basis).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();

                entity.HasOne(s => s.Shift)
                        .WithMany(sh => sh.Disposals)
                        .HasForeignKey(s => s.ShiftID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                entity.HasOne(s => s.Item)
                        .WithMany(i => i.Disposals)
                        .HasForeignKey(s => s.ItemID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.HasIndex(e => e.Barcode)
                        .IsUnique(true);
                entity.Property(e => e.Barcode)
                        .IsRequired();

                entity.Property(e => e.Name)
                        .IsRequired()
                        .HasMaxLength(50);

                entity.Property(e => e.Quantity)
                        .IsRequired()
                        .HasDefaultValue((float)0.0);

                entity.Property(e => e.Price)
                        .HasConversion<double>()
                        .IsRequired();

                entity.Property(e => e.Discount)
                        .IsRequired()
                        .HasDefaultValue(0);
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.Property(e => e.ID)
                    .ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.ShiftID)
                        .IsRequired();

                entity.Property(e => e.PostDateTime)
                        .IsRequired();

                entity.Property(e => e.TotalPrice)
                        .HasConversion<double>()
                        .IsRequired();

                entity.Property(e => e.ReceiptType)
                        .IsRequired();

                entity.HasOne(e => e.Shift)
                        .WithMany(t => t.Receipts)
                        .HasForeignKey(k => k.ShiftID)
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Receipt_to_Shift_by_ID")
                        .IsRequired();
            });

            modelBuilder.Entity<ReceiptItem>(entity =>
            {
                entity.HasKey(e => new { e.ReceiptID, e.ItemID });

                entity.Property(e => e.Quantity)
                        .IsRequired()
                        .HasDefaultValue((float)0.0);

                entity.Ignore(e => e.ItemName);

                entity.Property(e => e.Price)
                        .HasConversion<double>()
                        .IsRequired();

                entity.Property(e => e.Discount)
                        .IsRequired()
                        .HasDefaultValue(0);

                entity.HasOne(e => e.Item)
                        .WithMany(t => t.ReceiptItems)
                        .HasForeignKey(k => k.ItemID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ReceitItems_to_Items_by_ID")
                        .IsRequired();

                entity.HasOne(e => e.Receipt)
                        .WithMany(t => t.Items)
                        .HasForeignKey(k => k.ReceiptID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ReceitItems_to_Receipts_by_ID")
                        .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Login)
                        .IsRequired()
                        .HasMaxLength(50);
                entity.HasIndex(e => e.Login)
                        .IsUnique();

                entity.Property(e => e.Password)
                        .IsRequired()
                        .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                        .IsRequired()
                        .HasMaxLength(50);

                entity.Property(e => e.SecondName)
                        .IsRequired()
                        .HasMaxLength(50);

                entity.Property(e => e.ThirdName)
                        .IsRequired()
                        .HasMaxLength(50);

                entity.Property(e => e.Role)
                        .IsRequired();

                entity.Property(e => e.RegDateTime)
                        .IsRequired();
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.UserID)
                        .IsRequired();

                entity.Property(e => e.StartDateTime)
                        .IsRequired();

                entity.Property(e => e.EndDateTime);

                entity.Property(e => e.Balance)
                        .IsRequired()
                        .HasConversion<double>();

                entity.Property(e => e.AddsSum)
                        .IsRequired()
                        .HasConversion<double>();

                entity.Property(e => e.WithdrawalsSum)
                        .IsRequired()
                        .HasConversion<double>();
                entity.Property(e => e.SalesSum)
                        .IsRequired()
                        .HasConversion<double>();

                entity.Property(e => e.ReturnsSum)
                        .IsRequired()
                        .HasConversion<double>();

                entity.Property(e => e.State)
                        .IsRequired();

                entity.HasOne(e => e.User)
                        .WithMany(t => t.Shifts)
                        .HasForeignKey(k => k.UserID)
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
            });
        }

        #endregion

        #region User methods
        public async Task<bool> UpdateUser(IUser user)
        {
            if (!CanConnect)
            {
                return false;
            }
            var u = user as User;
            u.Password = SHA512ManagedHasher.GetHash(u.Password, Encoding.UTF8);
            Users.Update(u as User);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUser(IUser user)
        {
            if (!CanConnect)
            {
                return false;
            }

            Users.Remove(user as User);
            await SaveChangesAsync();
            return true;
        }

        public void SignIn(string login, string password)
        {
            if (!CanConnect)
            {
                throw new Exception("Нет соединения с базой данных!");
            }

            CurrentUser = Users.SingleOrDefault(u => u.Login == login && u.Password == password) ?? 
                                throw new Exception("Неверный логин/пароль!");

        }

        public User GetUserByLogin(string login)
        {
            if (!CanConnect)
            {
                return null;
            }

            return Users.FirstOrDefault(u => u.Login == login);
        }

        #endregion

        #region Item methods

        public Item GetItem(string barcode)
        {
            if (barcode == null || barcode == string.Empty)
            {
                throw new Exception("Товар не найден: попытка использования пустого значения штрих-кода!");
            }

            if (!CanConnect)
            {
                throw new Exception("Товар не найден: нет соединения с базой данных!");
            }

            return Items.FirstOrDefault(i => i.Barcode == barcode) ?? 
                    throw new Exception("Товар не найден: товар с введенным значением штрих-кода не существует!");
        }

        public async Task AddItem(IEditableItem item)
        {
            if (item == null)
            {
                throw new Exception("Регистрация товара не произведена: попытка использования null-объекта!");
            }

            if (!CanConnect)
            {
                throw new Exception("Регистрация товара не произведена: нет соединения с базой данных!");
            }

            if (Items.FirstOrDefault(i => i.Barcode == item.Barcode) != null)
            {
                throw new Exception("Регистрация товара не произведена: товар с указанным кодом уже существует!");
            }

            Items.Add(item as Item);
            await SaveChangesAsync();
        }

        public async Task UpdateItem(IEditableItem item)
        {
            if (item == null)
            {
                throw new Exception("Регистрация товара не произведена: попытка использования null-объекта!");
            }

            if (!CanConnect)
            {
                throw new Exception("Не удалось сохранить изменения: нет соединения с базой данных!");
            }

            if (Items.FirstOrDefault(i => i.Barcode == item.Barcode && i.ID != item.ID) != null)
            {
                throw new Exception("Не удалось сохранить изменения: иной товар с указанным кодом уже существует!");
            }

            Items.Update(item as Item);
            await SaveChangesAsync();
        }

        public async Task RemoveItem(IEditableItem item)
        {
            if (item == null)
            {
                throw new Exception("Не удалось удалить данные о товаре: попытка использования null-объекта!");
            }

            if (!CanConnect)
            {
                throw new Exception("Не удалось удалить данные о товаре: нет соединения с базой данных!");
            }

            Items.Remove(item as Item);
            await SaveChangesAsync();
        }

        #endregion

        #region Supply methods

        public async Task RegisterSupply(ISupply supply, IEditableItem item)
        {
            #region Conditions
            if (supply == null)
            {
                throw new Exception("Приёмка товара не произведена: попытка использования null-объекта!");
            }

            if (!CanConnect)
            {
                throw new Exception("Приёмка товара не произведена: нет соединения с базой данных!");
            }
            #endregion
            using (var transaction = await this.Database.BeginTransactionAsync())
            {
                var s = supply as Supply;
                var i = item as Item;
                try
                {
                    i.Quantity += s.Quantity;
                    Items.Update(i);
                    Supplies.Add(s);
                    await SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    i.Quantity -= s.Quantity;
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        #endregion

        #region Disposal methods

        public async Task RegisterDisposal(IDisposal disposal, ISellableItem item)
        {
            #region Conditions
            if (disposal == null)
            {
                throw new Exception("Списание товара не было осуществлено: попытка использования null-объекта!");
            }

            if (!CanConnect)
            {
                throw new Exception("Списание товара не было осуществлено: нет соединения с базой данных!");
            }

            if (item.Quantity < disposal.Quantity)
            {
                throw new Exception($"Списание товара не было осуществлено: нельзя списать больше, чем есть ({item.Quantity})!");
            }
            #endregion
            using (var transaction = await this.Database.BeginTransactionAsync())
            {
                var i = item as Item;
                var d = disposal as Disposal;
                try
                {
                    i.Quantity -= d.Quantity;
                    Items.Update(i);
                    Disposals.Add(d);
                    await SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    d.Item.Quantity += d.Quantity;
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        #endregion

        #region Shift methods

        public IShift GetLastUnfinishedShift(IUser user)
        {
            if (user == null)
            {
                throw new Exception("Не удалось восстановить смену: попытка использования null-объекта!");
            }

            if (!CanConnect)
            {
                throw new Exception("Не удалось восстановить смену: нет соединения с базой данных!");
            }

            return Shifts.FirstOrDefault(sh => sh.State == ShiftState.Running && sh.UserID == user.ID);
        }

        public async Task AddShift(IShift shift)
        {
            if (shift == null)
            {
                throw new Exception("Не удалось начать смену: попытка использования null-объекта!");
            }

            if (!CanConnect)
            {
                throw new Exception("Не удалось начать смену: нет соединения с базой данных!");
            }

            Shifts.Add(shift as Shift);
            await SaveChangesAsync();
        }

        public async Task UpdateShift(IShift shift)
        {
            if (shift == null)
            {
                throw new Exception("Не удалось сохранить изменения: попытка использования null-объекта!");
            }

            if (!CanConnect)
            {
                throw new Exception("Не удалось сохранить изменения: нет соединения с базой данных!");
            }

            Shifts.Update(shift as Shift);
            await SaveChangesAsync();
        }

        #endregion

        #region ReceiptMethods

        public Receipt GetReceipt(string receiptId)
        {
            if (!CanConnect)
            {
                throw new Exception("Чек не найден: нет соединения с базой данных!");
            }
            if (!long.TryParse(receiptId, out long id))
            {
                throw new Exception("Чек не найден: чек с введенным значением идентификатора не существует!");
            }
            return Receipts.AsNoTracking().Where((r => r.ID == id && r.ReceiptType == ReceiptType.Sale)).Include(r => r.Items).ThenInclude(r => r.Item).FirstOrDefault() ??
                    throw new Exception("Чек не найден: чек с введенным значением идентификатора не существует!");
        }

        #endregion

        #region Public methods
        public void InitializeDatabase()
        {
            this.Database.EnsureCreated();
            if (Users.Count(u => u.Role == Roles.Admin) < 1)
            {
                IUserEditor user = new User()
                {
                    Login = "root",
                    Password = "root",
                    Role = Roles.Admin,
                    FirstName = "root",
                    SecondName = "root",
                    ThirdName = "root",
                };
                user.Register();
            }
        }
        #endregion
    }
}
