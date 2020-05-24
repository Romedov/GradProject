using System;
using System.Threading.Tasks;
using Kassa.Models.Interfaces;

namespace Kassa.Models
{
    public class Supply : ISupply
    {
        public Supply(){}
        public Supply(IShift shift, ISellableItem item)
        {
            this.ShiftID = shift.ID;
            this.ItemID = item.ID;
            this.Basis = string.Empty;
            this.Quantity = 0;
            this.Price = 0;
            this.SupplyDT = default;
        }


        public long ID { get; private set; }
        public long ShiftID { get; private set; }
        public long ItemID { get; private set; }
        public string Basis { get; set; }
        public float Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime SupplyDT { get; private set; }

        #region Navigation props
        public Shift Shift { get; set; }
        public Item Item { get; set; }
        #endregion

        public async Task Register(IEditableItem item)
        {
            try
            {
                using (CashRegisterContext ctx = new CashRegisterContext())
                {
                    SupplyDT = DateTime.Now;
                    await ctx.RegisterSupply(this, item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}