using System.Collections.Generic;

namespace Kassa.Models.Interfaces
{
    public interface IEditableItem
    {
        #region Constructors
        #endregion
        #region Props
        long ID { get; }
        string Barcode { get; set; }
        string Name { get; set; }
        float Quantity { get; }
        decimal Price { get; set; }
        byte Discount { get; set; }
        IList<ReceiptItem> ReceiptItems { get; set; }
        IList<Supply> Supplies { get; set; }
        IList<Disposal> Disposals { get; set; }
        #endregion
    }
}
