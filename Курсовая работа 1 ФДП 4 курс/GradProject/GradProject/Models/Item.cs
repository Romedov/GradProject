using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Runtime.CompilerServices;
using System.Windows;
using GradProject.Models;
namespace GradProject
{
    public partial class Item : ItemParent, INotifyPropertyChanged //класс товаров, регистрируемых в БД
    {
        //#region Constructors
        //public Item() { }
        //public Item(decimal price)
        //{
        //    IId = "free";
        //    Price = price;
        //    Number = 1;
        //}
        //#endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public static event EventHandler<ItemSearchEventArgs> ItemSearching; //событие поиско товара в БД
        #endregion
        #region Private fields
        [NotMapped]
        private long _number; //количество
        #endregion
        #region Public properties
        [Key]
        [StringLength(50)]
        public override string IId{ get; protected set; } //id товара

        [Required]
        [StringLength(100)]
        public override string Name { get; protected set; } //наименование

        public override decimal Price { get; protected set; } //цена

        public override long Number //количество
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        public int Discount { get; private set; } //скидка

        #endregion
        #region Methods
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public static Item GetItem(object sender, string iId) //поиск товара в БД
        {
            using (CashboxDataContext db = new CashboxDataContext())
            {
                Item item = null;
                try
                {
                    db.DBConnectionCheck();
                    item = db.Items.FirstOrDefault(i => i.IId == iId);
                    if (item != null && item.Number >= 1)
                    {
                        item.Number = 1;
                        ItemSearching?.Invoke(sender, new ItemSearchEventArgs($"Товар с идентификатором {iId} добавлен в чек!", true));
                        return item;
                    }
                    else
                    {
                        ItemSearching?.Invoke(sender, new ItemSearchEventArgs($"Товар с идентификатором {iId} отсутствует на складе, либо не зарегистрирован в базе данных !", false));
                        return null;
                    }

                }
                catch (Exception e)
                {
                    ItemSearching?.Invoke(sender, new ItemSearchEventArgs($"Не удалось найти товар в базе данных!\n{e.Message}", false));
                    return null;
                }
            }
        }
        public bool NumberCheck() //проверка оставшегося количества
        {
            using(CashboxDataContext db = new CashboxDataContext())
            {
                long _checkedNum;
                try
                {
                    db.DBConnectionCheck();
                    _checkedNum = db.Items.First(i => i.IId == this.IId).Number;
                    if (_checkedNum < this.Number)
                    {
                        MessageBox.Show($"На складе не хватает {this.Number - _checkedNum} единиц товара \"{this.Name}\"!");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
        }
        public override async void SellItemAsync(Shift currShift) //асинхронная продажа
        {
            using(CashboxDataContext db = new CashboxDataContext())
            {
                try
                {
                    db.DBConnectionCheck();
                    Sale sale = db.Sales.FirstOrDefault(s => s.SId == currShift.SId && s.IId == this.IId);
                    Item item = db.Items.First(i => i.IId == this.IId);
                    if (sale == null)
                    {
                        db.Sales.Add(new Sale(currShift, this));
                    }
                    else
                    {
                        sale.Number += this.Number;
                    }
                    item.Number -= this.Number;
                    db.Shifts.Attach(currShift);
                    currShift.CashReceived += this.Price*this.Number;
                    currShift.CurrentCash += this.Price*this.Number;
                    await db.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public override async void ReturnItemAsync(Shift currShift) //асинхронный возврат
        {
            using (CashboxDataContext db = new CashboxDataContext())
            {
                try
                {
                    db.DBConnectionCheck();
                    Return _return = db.Returns.FirstOrDefault(r => r.SId == currShift.SId && r.IId == this.IId);
                    Item item = db.Items.First(i => i.IId == this.IId);
                    if (_return == null)
                    {
                        db.Returns.Add(new Return(currShift, this));
                    }
                    else
                    {
                        _return.Number += this.Number;
                    }
                    item.Number += this.Number;
                    db.Shifts.Attach(currShift);
                    currShift.CashReturned += this.Price * this.Number;
                    currShift.CurrentCash -= this.Price * this.Number;
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion
    }
}
