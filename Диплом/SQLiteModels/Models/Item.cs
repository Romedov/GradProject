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
        public Item(string barcode, string name, float quantity, decimal price, byte discount)
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
        public event EventHandler<ItemChangesAppliedEventArgs> ChangesApplied;
        public event EventHandler<ItemRegisteredEventArgs> Registered;
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
                _name = value.Trim();
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
        
        public IEnumerable<ReceiptItem> ReceiptItems { get; private set; }
        public IEnumerable<Supply> Supplies { get; private set; }
        public IEnumerable<Disposal> Disposals { get; private set; }
        
        #endregion
        
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        public void ApplyChanges()
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                if (ctx.CanConnect)
                {
                    ctx.Items.Update(this);
                    ctx.SaveChanges();
                    ChangesApplied?.Invoke(this, new ItemChangesAppliedEventArgs("Изменения сохранены!"));
                }
                else
                {
                    ChangesApplied?.Invoke(this, 
                        new ItemChangesAppliedEventArgs(
                            "Не удалось сохранить изменения: нет соединения с базой данных!", false));
                }
                
            }
        }

        public void Register()
        {
            if (Barcode == string.Empty || Name == string.Empty)
            {
                Registered?.Invoke(this, new ItemRegisteredEventArgs("Присутствуют незаполненные поля!", false));
                return;
            }

            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                if (ctx.CanConnect)
                {
                    ctx.Items.Add(this);
                    ctx.SaveChanges();
                    Registered?.Invoke(this, new ItemRegisteredEventArgs("Товар зарегистрирован!"));
                }
                else
                {
                    Registered?.Invoke(this, new ItemRegisteredEventArgs("Нет соединения с базой данных!", false));
                }
            }
        }
        public static Item GetItemByBarCode(string barcode)
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                return ctx.Items.FirstOrDefault(i => i.Barcode == barcode);
            }
        }
        public static Item GetItemByID(int id)
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                return ctx.Items.FirstOrDefault(i => i.ID == id);
            }
        }
        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
