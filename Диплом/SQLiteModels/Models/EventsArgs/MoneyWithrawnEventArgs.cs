using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class MoneyWithrawnEventArgs : EventArgs
    {
        #region Constructros
        public MoneyWithrawnEventArgs(string message, bool successful = true)
        {
            this.Message = message;
            this.Successful = successful;
        }
        #endregion
        #region Public props
        public bool Successful { get; private set; }
        public string Message { get; private set; }
        #endregion
    }
}
