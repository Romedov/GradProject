using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.Interfaces
{
    public interface ISellableItem
    {
        #region Constructors
        #endregion
        #region Events
        #endregion
        #region Props
        long ID { get; }
        string Barcode { get; }
        string Name { get; }
        float Quantity { get; }
        decimal Price { get; }
        byte Discount { get; }
        #endregion
        #region Methods
        #endregion
    }
}
