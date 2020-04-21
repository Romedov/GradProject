using EntityFrameworkCoreTest.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCoreTest.Models
{
    public class ReceiptItem : IItem
    {
        #region Constructors
        public ReceiptItem() { }
        public ReceiptItem(IItem item, float quantity) 
        {
            ID = item.ID;
            Name = item.Name;
            Quantity = quantity;
            Price = item.Price;
            Discount = item.Discount;
        }
        #endregion
        #region Events
        #endregion
        #region Private props
        #endregion
        #region Public props
        public int ID { get; private set; }
        public decimal ReceiptID { get; private set; }
        public string Name { get; private set; }
        public float Quantity { get; set; }
        public decimal Price { get; private set; }
        public byte Discount { get; private set; }
        public Item Item { get; private set; } //nav
        public Receipt Receipt { get; private set; } //nav
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        #endregion
    }
}
