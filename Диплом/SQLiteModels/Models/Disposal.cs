using System;
using Kassa.Models.EventsArgs;
using Kassa.Models.Interfaces;

namespace Kassa.Models
{
    public class Disposal : IDisposal
    {
        public event EventHandler<DisposalRegisteredEventArgs> Registered;
        
        public Disposal()
        {
            
        }

        public Disposal(IShift shift, ISellableItem item)
        {
            Shift = shift as Shift;
            ShiftID = shift.ID;
            Item = item as Item;
            ItemID = item.ID;
            Basis = string.Empty;
            Quantity = (float)0;
        }
        
        public long ShiftID { get; }
        public long ItemID { get; }
        public string Basis { get; set; }
        public float Quantity { get; set; }
        public DateTime DisposalDT { get; private set; }
        
        #region Navigation props
        public Shift Shift { get; }
        public Item Item { get; }
        #endregion
        
        public void Register()
        {
            if (this.Basis == string.Empty || this.Quantity == (float)0)
            {
                Registered?.Invoke(this, new DisposalRegisteredEventArgs("Одно из полей не заполнено!", false));
                return;
            }

            if (this.Quantity > Item.Quantity)
            {
                Registered?.Invoke(this, 
                    new DisposalRegisteredEventArgs(
                        $"Невозможно списать товаров больше, чем есть на складе ({Item.Quantity})!", 
                        false));
                return;
            }
            
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                if (ctx.CanConnect)
                {
                    this.DisposalDT = DateTime.Now;
                    Item.Quantity += this.Quantity;
                    ctx.Items.Update(Item);
                    ctx.Disposals.Add(this);
                    ctx.SaveChanges();
                    Registered?.Invoke(this, new DisposalRegisteredEventArgs("Товар списан!"));
                }
                else
                {
                    Registered?.Invoke(this, new DisposalRegisteredEventArgs("Нет соединения с базой данных!", false));
                }
            }
        }
    }
}