using Kassa.Models.Interfaces;
using Kassa.Models.EventsArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public enum ReceiptType    //тип чека
    {
        Sale = 0,       //продажа
        Return = 1,     //возврат
    }
    public class Receipt : IPostable
    {
        #region Constructors
        public Receipt()
        {
            this.Items = new ObservableCollection<ReceiptItem>();
        }
        public Receipt(ReceiptType type)
        {
            this.Items = new ObservableCollection<ReceiptItem>();
            this.Items.CollectionChanged += this.OnItemsChanged;
            this.ReceiptType = type;
            this.TotalPrice = 0;
        }
        #endregion
        #region Events
        public event EventHandler<ReceiptPostedEventArgs> ReceiptPosted;
        #endregion
        #region Private props
        #endregion
        #region Public props
        public decimal ID { get; private set; }
        public decimal ShiftID { get; private set; }
        public DateTime PostDateTime { get; private set; }
        public ObservableCollection<ReceiptItem> Items { get; private set; } 
        public decimal TotalPrice { get; private set; }
        public ReceiptType ReceiptType { get; private set; }
        public Shift Shift { get; private set; }                  //nav prop
        #endregion
        #region Private methods
        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: // если добавление
                    
                    break;
                case NotifyCollectionChangedAction.Remove: // если удаление
                    
                    break;
                case NotifyCollectionChangedAction.Replace: // если замена
                    
                    break;
            }
        }
        private bool ContainsItem(ISellableItem item, out int index) //проверка на наличие указанного товара в чеке
        {
            index = this.Items.ToList().FindIndex(i => i.ItemID == item.ID);
            if (index >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ComputeTotalPrice() //подстчет итоговой цены чека
        {
            this.TotalPrice = this.Items.Sum(i => i.GetTotalPrice());
        }
        #endregion
        #region Public methods
        public void AddItem(ISellableItem item) //добавление товара в чек
        {
            if (this.ContainsItem(item, out int index))
            {
                this.Items[index].Quantity++;
            }
            else
            {
                this.Items.Add(new ReceiptItem(item));
            }
            this.ComputeTotalPrice();
        }
        public void RemoveItem(ReceiptItem receiptItem) //удаление товара из чека
        {
            this.Items.Remove(receiptItem);
            this.ComputeTotalPrice();
        }
        public void AssignShiftAndDate(IShift shift)
        {
            this.ShiftID = shift.ID;
            this.Shift = shift as Shift;
            this.PostDateTime = DateTime.Now;
        }
        public bool CanPost(IShift shift, decimal money, out string message) //возможно ли проведение чека продажи
        {
            if ((money >= this.TotalPrice) && (this.Items.Count > 0) && ((money - this.TotalPrice) <= shift.Balance))
            {
                message = $"Сдача: {money - this.TotalPrice} руб.";
                return true;
            }
            else if (money < this.TotalPrice)
            {
                message = $"к оплате предоставлено недостаточно средств ({this.TotalPrice - money})";
                return false;
            }
            else if (this.Items.Count < 1)
            {
                message = $"список товаров пуст";
                return false;
            }
            else
            {
                message = $"недостаточно средств для выдачи сдачи " +
                    $"({(money - this.TotalPrice) - shift.Balance}).\n " +
                    $"В кассе: {shift.Balance} руб)";
                return false;
            }
        }
        public bool CanPost(IShift shift, out string message) //возможно ли проведение чека возврата
        {
            if ((this.Items.Count > 0) && (this.TotalPrice <= shift.Balance))
            {
                message = $"К возврату: {this.TotalPrice} руб.";
                return true;
            }
            else if (this.Items.Count < 1)
            {
                message = $"список товаров пуст";
                return false;
            }
            else
            {
                message = $"недостаточно средств в кассе для выдачи сдачи " +
                    $"({this.TotalPrice - shift.Balance}).\n " +
                    $"В кассе: {shift.Balance} руб)";
                return false;
            }
        }
        public decimal Post(IPost poster, IShift shift, decimal money = 0)
        {
            var result = poster.TryPost(shift, out string message, money);
            if (result)
            {
                this.ReceiptPosted?.Invoke(this, new ReceiptPostedEventArgs($"Чек проведён. {message}"));
                return this.TotalPrice;
            }
            else
            {
                this.ReceiptPosted?.Invoke(this,
                    new ReceiptPostedEventArgs($"Чек не проведён: {message}.", false));
                return 0;
            }
        }

        //public decimal PostSale(IShift shift, decimal money) //проведение чека
        //{
        //    if (this.CanPostSale(shift, money, out string message))
        //    {
        //        using (CashRegisterContext ctx = new CashRegisterContext())
        //        {
        //            if (ctx.CheckConnection(out string dbMessage))
        //            {
        //                this.AssignPostData(shift);
        //                ctx.Receipts.Add(this);
        //                ctx.SaveChanges();
        //                this.ReceiptPosted?.Invoke(this, new ReceiptPostedEventArgs($"Чек проведён. {message}"));
        //                return this.TotalPrice;
        //            }
        //            else
        //            {
        //                this.ReceiptPosted?.Invoke(this,
        //                        new ReceiptPostedEventArgs($"Чек не проведён. {dbMessage}.", false));
        //                return 0;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        this.ReceiptPosted?.Invoke(this,
        //            new ReceiptPostedEventArgs($"Чек не проведён: {message}.", false));
        //        return 0;
        //    }
        //}












        //public decimal PostReturn(IShift shift) //проведение чека
        //{
        //    if (this.ReceiptType != ReceiptType.Return)
        //    {
        //        return;
        //    }

        //    if (this.CanPostReturn(shift, out string message))
        //    {
        //        using (CashRegisterContext ctx = new CashRegisterContext())
        //        {
        //            if (ctx.CheckConnection(out string dbMessage))
        //            {
        //                this.AssignPostData(shift);
        //                ctx.Receipts.Add(this);
        //                ctx.SaveChanges();
        //                this.ReceiptPosted?.Invoke(this, new ReceiptPostedEventArgs($"Чек проведён. {message}"));
        //                return this.TotalPrice;
        //            }
        //            else
        //            {
        //                this.ReceiptPosted?.Invoke(this,
        //                        new ReceiptPostedEventArgs($"Чек не проведён. {dbMessage}.", false));
        //                return 0;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        this.ReceiptPosted?.Invoke(this,
        //            new ReceiptPostedEventArgs($"Чек не проведён: {message}.", false));
        //        return 0;
        //    }
        //}
        #endregion
    }
}
