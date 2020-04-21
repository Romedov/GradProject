using EntityFrameworkCoreTest.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCoreTest.Models
{
    public enum ReceiptType    //тип чека
    {
        Sale = 0,       //продажа
        Return = 1,     //возврат
    }
    public class Receipt
    {
        #region Constructors
        #endregion
        #region Events
        #endregion
        #region Private props
        #endregion
        #region Public props
        public decimal ID { get; set; }
        public decimal ShiftID { get; set; }
        public DateTime PostDateTime { get; set; }
        public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
        public decimal TotalPrice { get; set; }
        public ReceiptType ReceiptType { get; set; }
        public Shift Shift { get; set; }                  //nav prop
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        #endregion
    }
}
