using Kassa.Models.EventsArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.Interfaces
{
    public interface IPostable
    {
        #region Events
        event EventHandler<ReceiptPostedEventArgs> ReceiptPosted;
        #endregion
        #region Props
        decimal TotalPrice { get; }
        ReceiptType ReceiptType { get; }
        #endregion
        #region Methods
        void Post(IShift shift, decimal money = 0);
        int GetItemsCount();
        void AssignShiftAndDate(IShift shift);
        #endregion
    }
}
