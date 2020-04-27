using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Kassa.Models
{
    public class ReceiptItem
    {
        #region Constructors
        public ReceiptItem() { }
        public ReceiptItem(ISellableItem item) 
        {
            this.Item = item as Item;
            this.ItemID = item.ID;
            this.ItemName = item.Name;
            this.Quantity = 1;
            this.Price = item.Price;
            this.Discount = item.Discount;
        }
        #endregion
        #region Events
        #endregion
        #region Private props
        #endregion
        #region Public props
        public long ReceiptID { get; private set; }
        public long ItemID { get; private set; }
        public string ItemName { get; private set; }
        public float Quantity { get; set; }
        public decimal Price { get; private set; }
        public byte Discount { get; private set; }
        public Item Item { get; private set; } //nav
        public Receipt Receipt { get; private set; } //nav
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        public decimal GetTotalPrice()
        {
            return this.Price * (1 - this.Discount / 100) * Convert.ToDecimal(this.Quantity);
        }
        public bool IsEnough(out float difference)
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
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
