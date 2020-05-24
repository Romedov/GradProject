using System.Collections.Generic;

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
        IList<ReceiptItem> ReceiptItems { get; set; }
        IList<Supply> Supplies { get; set; }
        IList<Disposal> Disposals { get; set; }
        #endregion
        #region Methods
        #endregion
    }
}
