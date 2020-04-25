using Kassa.Models.Interfaces;
using Kassa.Models.EventsArgs;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

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
        private void InitializePoster(IPoster poster, IShift shift, decimal money = 0)
        {
            if (poster.TryPost(this, shift, out string message, money))
            {
                this.ReceiptPosted?.Invoke(this,
                            new ReceiptPostedEventArgs($"Готово: {message}."));
            }
            else
            {
                this.ReceiptPosted?.Invoke(this,
                            new ReceiptPostedEventArgs($"Ошибка: {message}.", false));
            }
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
        public void Post(IShift shift, decimal money = 0)
        {
            switch (this.ReceiptType)
            {
                case ReceiptType.Sale:
                    this.InitializePoster(new SalePoster(), shift, money);
                    break;
                case ReceiptType.Return:
                    this.InitializePoster(new ReturnPoster(), shift);
                    break;
                default:
                    this.ReceiptPosted?.Invoke(this,
                            new ReceiptPostedEventArgs($"Ошибка: не задан тип операции (продажа/возврат).", false));
                    break;
            }
        }
        public int GetItemsCount()
        {
            return this.Items.Count;
        }
        #endregion
    }
}
