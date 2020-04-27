using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models
{
    public class Supply
    {
        #region Constructors
        #endregion
        #region Events
        #endregion
        #region Private props
        #endregion
        #region Public props
        public decimal ID { get; private set; }
        public string SupplierName { get; set; }
        public int ItemID { get; set; }
        public float Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SupplyDateTime { get; set; }
        public decimal ShiftID { get; set; }
        public Item Item { get; private set; } //nav
        public Shift Shift { get; private set; } //nav
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        #endregion
    }
}
