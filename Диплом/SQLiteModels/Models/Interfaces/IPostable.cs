using Kassa.Models.EventsArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kassa.Models.Interfaces
{
    public interface IPostable
    {
        #region Events
        event EventHandler<ReceiptPostedEventArgs> ReceiptPosted;
        #endregion
        #region Props
        ObservableCollection<ReceiptItem> Items { get; }
        decimal TotalPrice { get; }
        ReceiptType ReceiptType { get; }
        #endregion
        #region Methods
        void AddItem(ISellableItem item);
        void RemoveItem(ReceiptItem receiptItem);
        void Post(IShift shift, decimal money = 0);
        int GetItemsCount();
        void AssignShiftAndDate(IShift shift);
        #endregion
    }
}
