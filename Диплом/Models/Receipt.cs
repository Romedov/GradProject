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
    public class Receipt
    {
        #region Constructors
        public Receipt()
        {
            this.Items = new ObservableCollection<ReceiptItem>();
        }
        public Receipt(Shift shift, ReceiptType type)
        {
            this.Items = new ObservableCollection<ReceiptItem>();
            this.Items.CollectionChanged += this.OnItemsChanged;
            this.ShiftID = shift.ID;
            this.Shift = shift;
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
        private bool CanPost(decimal money, out string message)
        {
            if (money >= this.TotalPrice && this.Items.Count > 0)
            {
                message = $"Сдача: {this.TotalPrice - money} руб.";
                return true;
            }
            else if (money < this.TotalPrice)
            {
                message = $"не достаточно средств ({this.TotalPrice - money})";
                return false;
            }
            else 
            {
                message = $"список товаров пуст";
                return false;
            }
            
        }
        #endregion
        #region Public methods
        public void AddItem(ISellableItem item) //добавление товара в чек
        {
            int index;
            if (this.ContainsItem(item, out index))
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
        public void Post(decimal money)
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                string message;
                if (this.CanPost(money, out message))
                {
                    ctx.Receipts.Add(this);
                    ctx.SaveChanges();
                    this.ReceiptPosted?.Invoke(this, new ReceiptPostedEventArgs($"Чек проведён. {message}"));
                }
                else
                {
                    this.ReceiptPosted?.Invoke(this,
                        new ReceiptPostedEventArgs($"Чек не проведён: {message}.", false));
                }

                if (this.Items.Count > 0)
                {
                    ctx.Receipts.Add(this);
                    ctx.SaveChanges();
                    this.ReceiptPosted?.Invoke(this, new ReceiptPostedEventArgs("Чек проведён."));
                }
                else
                {
                    this.ReceiptPosted?.Invoke(this, 
                        new ReceiptPostedEventArgs("Чек не проведён, т.к. список товаров чека оказался пуст.", false));
                }
            }
        }
        #endregion
    }
}
