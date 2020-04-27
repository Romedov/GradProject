using System;
using Kassa.Models.EventsArgs;
using Kassa.Models.Interfaces;

namespace Kassa.Models
{
    public class Supply : ISupply
    {
        public Supply(){}
        public Supply(IShift shift, ISellableItem item)
        {
            this.Shift = shift as Shift;
            this.ShiftID = shift.ID;
            this.Item = item as Item;
            this.ItemID = item.ID;
            this.Basis=string.Empty;
            this.Quantity = (float)0;
            this.Price = 0;
            this.SupplyDT = default(DateTime);
        }

        public event EventHandler<SupplyRegisteredEventArgs> Registered;
        
        public long ShiftID { get; }
        public long ItemID { get; }
        public string Basis { get; set; }
        public float Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime SupplyDT { get; private set; }

        #region Navigation props
        public Shift Shift { get; }
        public Item Item { get; }
        #endregion
        
        public void Register()
        {
            if (this.Basis == string.Empty || this.Quantity == (float)0 || this.Price == 0)
            {
                Registered?.Invoke(this, new SupplyRegisteredEventArgs("Одно из полей не заполнено!", false));
                return;
            }
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                if (ctx.CanConnect)
                {
                    this.SupplyDT = DateTime.Now;
                    ctx.Supplies.Add(this);
                    ctx.SaveChanges();
                    Registered?.Invoke(this, new SupplyRegisteredEventArgs("Приемка произведена!"));
                }
                else
                {
                    Registered?.Invoke(this, new SupplyRegisteredEventArgs("Не удалось подключиться к базе данных!", false));
                }
            }
        }
    }
}