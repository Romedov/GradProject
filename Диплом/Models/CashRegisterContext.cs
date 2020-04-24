using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Kassa.Models
{
    public class CashRegisterContext : DbContext
    {
        #region Public constructors
        public CashRegisterContext()
        {
            
        }
        #endregion
        #region Public events

        #endregion
        #region Public props
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ReceiptItem> ReceiptItems { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<Supply> Supplies { get; set; }
        #endregion
        #region Protected methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=127.0.0.1,1433;" +
                                            "Initial Catalog=TestShopDB;" +
                                            "User ID=romedov;" +
                                            "Password=1243;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Supply>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnType(@"decimal(38,0)")
                    .ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Quantity)
                        .IsRequired();

                entity.Property(e => e.ItemID)
                        .IsRequired();

                entity.Property(e => e.SupplierName)
                        .IsRequired();

                entity.Property(e => e.TotalPrice)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

                entity.Property(e => e.SupplyDateTime)
                        .IsRequired()
                        .HasColumnType(@"datetime");

                entity.Property(e => e.ShiftID)
                        .IsRequired();

                entity.HasOne(e => e.Item)
                        .WithMany(t => t.Supplies)
                        .HasForeignKey(k => k.ItemID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Supply_to_Item_by_ID")
                        .IsRequired();

                entity.HasOne(e => e.Shift)
                        .WithMany(t => t.Supplies)
                        .HasForeignKey(k => k.ShiftID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Supply_to_Shift_by_ID")
                        .IsRequired();
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Barcode)
                        .IsRequired()
                        .HasMaxLength(50);
                entity.HasIndex(e => e.Barcode)
                        .IsUnique();

                entity.Property(e => e.Name)
                        .IsRequired()
                        .HasMaxLength(50);

                entity.Property(e => e.Quantity)
                        .IsRequired()
                        .HasDefaultValue((float)0.0);

                entity.Property(e => e.Price)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

                entity.Property(e => e.Discount)
                        .IsRequired()
                        .HasDefaultValue(0);
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnType(@"decimal(38,0)")
                    .ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.ShiftID)
                        .IsRequired();

                entity.Property(e => e.PostDateTime)
                        .IsRequired()
                        .HasColumnType(@"datetime");

                entity.Property(e => e.TotalPrice)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

                entity.Property(e => e.ReceiptType)
                        .IsRequired()
                        .HasColumnType(@"nvarchar(20)");

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

                entity.Property(e => e.Price)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

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
                        .IsRequired()
                        .HasColumnType(@"nvarchar(20)");

                entity.Property(e => e.RegDateTime)
                        .IsRequired()
                        .HasColumnType(@"datetime");
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd()
                        .HasColumnType(@"decimal(38,0)");
                entity.HasKey(e => e.ID);

                entity.Property(e => e.UserID)
                        .IsRequired();

                entity.Property(e => e.StartDateTime)
                        .IsRequired()
                        .HasColumnType(@"datetime");

                entity.Property(e => e.EndDateTime)
                        .HasColumnType(@"datetime");

                entity.Property(e => e.Balance)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

                entity.Property(e => e.AddsSum)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

                entity.Property(e => e.WithdrawalsSum)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

                entity.Property(e => e.SalesSum)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

                entity.Property(e => e.ReturnsSum)
                        .IsRequired()
                        .HasColumnType(@"decimal(18,2)");

                entity.HasOne(e => e.User)
                        .WithMany(t => t.Shifts)
                        .HasForeignKey(k => k.UserID)
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
            });
        }
        #endregion
        #region Public methods
        public bool CheckConnection(out string message)
        {
            try
            {
                if (this.Database.CanConnect())
                {
                    message = "Подключение установлено.";
                    return true;
                }
                else
                {
                    message = "Не удалось подключиться к базе данных.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = $"Не удалось подключиться к базе данных: {ex.Message}";
                return false;
            }
        }
        #endregion
    }
}
