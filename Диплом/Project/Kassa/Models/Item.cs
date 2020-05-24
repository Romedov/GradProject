using System;
using System.Collections.Generic;
using System.ComponentModel;
using Kassa.Models.Interfaces;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Kassa.Models.EventsArgs;

namespace Kassa.Models
{
    public class Item : ISellableItem, IEditableItem, INotifyPropertyChanged
    {
        #region Constructors

        public Item()
        {
            this.Barcode = string.Empty;
            this.Name = string.Empty;
            this.Price = 0;
            this.Quantity = 0;
            this.Discount = 0;
        }
        public Item(string barcode, string name, decimal price, byte discount)
        {
            this.Barcode = barcode;
            this.Name = name;
            this.Quantity = 0;
            this.Price = price;
            this.Discount = discount;
        }
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Private props
        
        private string _barcode;
        private string _name;
        private float _quantity;
        private decimal _price;
        private byte _discount;
        
        #endregion
        #region Public props
        
        public long ID { get; private set; }
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value.Replace(" ", "");
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public float Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value < 0 ? _quantity = 0 : _quantity = value;
                OnPropertyChanged();
            }
        }
        public decimal Price
        {
            get => _price;
            set
            {
                _price = _price < 0 ? _price = 0 : _price = value;
                OnPropertyChanged();
            }
        }
        public byte Discount
        {
            get => _discount;
            set
            {
                _discount = value > 100 ? _discount = 100 : _discount = value;
                OnPropertyChanged();
            }
        }

        #region Navigation props

        public IList<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();
        public IList<Supply> Supplies { get; set; } = new List<Supply>();
        public IList<Disposal> Disposals { get; set; } = new List<Disposal>();

        #endregion

        #endregion
        #region Protected methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
