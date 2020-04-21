using System;
using System.Collections.Generic;
using System.Text;
using EntityFrameworkCoreTest.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreTest.Models
{
    public class Item : IItem
    {
        #region Constructors
        public Item() { }
        public Item(string barcode, string name, float quantity, decimal price, byte discount)
        {
            Barcode = barcode;
            Name = name;
            Quantity = quantity;
            Price = price;
            Discount = discount;
        }
        #endregion
        #region Events
        #endregion
        #region Private props
        #endregion
        #region Public props
        public int ID { get; private set; }
        public string Barcode { get; private set; }
        public string Name { get; private set; }
        public float Quantity { get; set; }
        public decimal Price { get; private set; }
        public byte Discount { get; private set; }
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; } //nav
        public IEnumerable<Supply> Supplies { get; set; } //nav
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        #endregion
    }
}
