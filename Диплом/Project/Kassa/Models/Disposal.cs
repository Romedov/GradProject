using System;
using System.Threading.Tasks;
using Kassa.Models.Interfaces;

namespace Kassa.Models
{
    public class Disposal : IDisposal
    {
        
        public Disposal()
        {
            
        }

        public Disposal(IShift shift, ISellableItem item)
        {
            ShiftID = shift.ID;
            ItemID = item.ID;
            Basis = string.Empty;
            Quantity = 0;
        }

        public long ID { get; private set; }
        public long ShiftID { get; private set; }
        public long ItemID { get; private set; }
        public string Basis { get; set; }
        public float Quantity { get; set; }
        public DateTime DisposalDT { get; private set; }

        #region Navigation props
        public Shift Shift { get; set; }
        public Item Item { get; set; }
        #endregion

        public async Task Register(ISellableItem item)
        {
            try
            {
                using (CashRegisterContext ctx = new CashRegisterContext())
                {
                    DisposalDT = DateTime.Now;
                    await ctx.RegisterDisposal(this, item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}