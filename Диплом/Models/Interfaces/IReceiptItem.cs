using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCoreTest.Models.Interfaces
{
    public interface IReceiptItem
    {
        #region Constructors
        #endregion
        #region Events
        #endregion
        #region Props
        int ID { get; }
        float Quantity { get; set; }
        decimal Price { get; }
        byte Discount { get; }
        #endregion
        #region Methods
        #endregion
    }
}
