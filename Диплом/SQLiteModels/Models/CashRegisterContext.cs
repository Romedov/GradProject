using System;
using System.Collections.Generic;
using System.Text;
using Kassa.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kassa.Models
{
    public class CashRegisterContext : DbContext
    {
        //TODO: создать функционал списания товаров
        //TODO: добавить/изменить проверку подключения на "CanConnect"
        //TODO: создать дополнительные события 
        #region Public constructors
        public CashRegisterContext()
        {
            this.CanConnect = (this.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists() && this.Database.CanConnect();
        }
        #endregion
        #region Public events

        #endregion
        #region Public props
        public bool CanConnect { get; }
        public virtual DbSet<Item> Items { get; private set; }
        public virtual DbSet<ReceiptItem> ReceiptItems { get; private set; }
        public virtual DbSet<Shift> Shifts { get; private set; }
        public virtual DbSet<User> Users { get; private set; }
        public virtual DbSet<Receipt> Receipts { get; private set; }
        public virtual DbSet<Supply> Supplies { get; private set; }
        #endregion
        #region Protected methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=Shop.db");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Supply>(entity =>
            {
                    entity.HasKey(e => new {e.ShiftID, e.ItemID});
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

                entity.Property(e => e.IsFinished)
                        .IsRequired();

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
        
        public void InitializeDatabase()
        {
            if (this.CanConnect)
            {
                this.Database.Migrate();
                this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
                IUserEditor user = new User();
                user.Login = "root";
                user.Password = "1243";
                user.Role = Roles.Admin;
                user.FirstName = user.SecondName = user.ThirdName = "root";
                user.Register();
            }
            else
            {
                throw new Exception("Не удалось инициализировать базу данных");
            }
        }
        #endregion
    }
}
