using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GradProject
{
    public partial class CashboxDataContext : DbContext //класс-кнотекст данных
    {
        public CashboxDataContext()
            : base("name=CashboxDataContext")
        {
        }

        public virtual DbSet<FreeItem> FreeItems { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Return> Returns { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public void DBConnectionCheck() //проверка соединения с БД
        {
            if (!this.Database.Exists())
            {
                throw new Exception("Нет соединения с базой данных!");
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(e => e.IId)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Return>()
                .Property(e => e.IId)
                .IsUnicode(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.IId)
                .IsUnicode(false);

            modelBuilder.Entity<Shift>()
                .Property(e => e.UId)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UId)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.SurName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FatherName)
                .IsUnicode(false);
        }
    }
}
