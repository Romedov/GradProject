using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class ReceiptPostedEventArgs : EventArgs
    {
        #region Constructros
        public ReceiptPostedEventArgs(string message, bool isPosted = true)
        {
            this.Message = message;
            this.IsPosted = isPosted;
        }
        #endregion
        #region Public props
        public bool IsPosted { get; private set; }
        public string Message { get; private set; }
        #endregion
    }
}
