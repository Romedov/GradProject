using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kassa.Models
{
    public class ReceiptItem
    {
        #region Constructors
        public ReceiptItem() { }
        public ReceiptItem(ISellableItem item) 
        {
            this.Item = item as Item;
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
        public decimal ReceiptID { get; private set; }
        public int ItemID { get; private set; }
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
        #endregion
    }
}
