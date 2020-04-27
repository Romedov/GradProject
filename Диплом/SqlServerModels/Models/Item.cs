using System;
using System.Collections.Generic;
using Kassa.Models.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class Item : ISellableItem, IEditableItem
    {
        #region Constructors
        public Item() { }
        public Item(string barcode, string name, float quantity, decimal price, byte discount)
        {
            this.Barcode = barcode;
            this.Name = name;
            this.Quantity = quantity;
            this.Price = price;
            this.Discount = discount;
        }
        #endregion
        #region Events
        #endregion
        #region Private props
        #endregion
        #region Public props
        public int ID { get; private set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        public IEnumerable<ReceiptItem> ReceiptItems { get; private set; } //nav
        public IEnumerable<Supply> Supplies { get; private set; } //nav
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        public decimal GetTotalPrice()
        {
            return this.Price * (1 - this.Discount / 100) * Convert.ToDecimal(this.Quantity);
        }
        public async Task ApplyChanges()
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                ctx.Items.Update(this);
                await ctx.SaveChangesAsync();
            }
        }
        public static Item GetItemByBarCode(string barcode)
        {
            Item item = null;
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                item = ctx.Items.FirstOrDefault(i => i.Barcode == barcode);
            }
            return item;
        }
        public static Item GetItemByID(int id)
        {
            Item item = null;
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                item = ctx.Items.FirstOrDefault(i => i.ID == id);
            }
            return item;
        }
        #endregion
        #region Operators
        #endregion
    }
}
