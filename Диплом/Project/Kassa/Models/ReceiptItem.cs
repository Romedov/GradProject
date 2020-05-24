using Kassa.Models.Interfaces;
using System;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kassa.Models
{
    public class ReceiptItem : INotifyPropertyChanged
    {
        #region Constructors
        public ReceiptItem() { }
        public ReceiptItem(ISellableItem item) 
        {
            this.ItemID = item.ID;
            this.ItemName = item.Name;
            this.Quantity = 1;
            this.Price = item.Price;
            this.Discount = item.Discount;
            this.Item = item as Item;
        }
        public ReceiptItem(ReceiptItem item)
        {
            this.ItemID = item.ItemID;
            this.Item = item.Item;
            //this.ItemName = item.ItemName;
            this.Quantity = item.Quantity;
            this.Price = item.Price;
            this.Discount = item.Discount;
        }
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Private props
        private string _itemName;
        private float _quantity;
        private decimal _price;
        private byte _discount;
        private Item _item;
        #endregion
        #region Public props
        public long ReceiptID { get; private set; }
        public long ItemID { get; private set; }
        public string ItemName 
        {
            get => _itemName;
            private set
            {
                _itemName = value ?? string.Empty;
                OnPropertyChanged();
            }
        }
        public float Quantity 
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public decimal Price 
        {
            get => _price;
            private set
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public byte Discount 
        {
            get => _discount;
            private set
            {
                _discount = value;
                OnPropertyChanged();
            }
        }
        public Item Item //nav
        {
            get => _item;
            set
            {
                _item = value;
                if (value != null)
                {
                    ItemName = value.Name ?? string.Empty;
                }
                
                //_item = null;
            }
        } 
        public Receipt Receipt { get; set; } //nav
        #endregion
        #region Private methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Public methods
        public decimal GetTotalPrice()
        {
            decimal total = this.Price * Convert.ToDecimal(this.Quantity) * (1m - Convert.ToDecimal(this.Discount) / 100m);
            return total;
        }
        public bool IsEnough(out float difference)
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                if (!ctx.CanConnect)
                {
                    difference = 0;
                    return false;
                }
                ISellableItem item = ctx.Items.FirstOrDefault(i => i.ID == this.ItemID);
                if (item!=null)
                {
                    if (this.Quantity <= item.Quantity)
                    {
                        difference = item.Quantity - this.Quantity;
                        return true;
                    }
                    else
                    {
                        difference = this.Quantity - item.Quantity;
                        return false;
                    }
                }
                else
                {
                    difference = 0;
                    return false;
                }
            }
        }
        #endregion
    }
}
