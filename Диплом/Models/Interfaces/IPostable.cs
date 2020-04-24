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
        decimal Post(IPost poster, IShift shift, decimal money = 0);
        bool CanPost(IShift shift, out string message);
        bool CanPost(IShift shift, decimal money, out string message);
        #endregion
    }
}
