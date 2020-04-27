using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class ShiftStartedEventArgs : EventArgs
    {
        #region Constructros
        public ShiftStartedEventArgs(string message, bool isRunning = true)
        {
            this.Message = message;
            this.IsRunning = isRunning;
        }
        #endregion
        #region Public props
        public bool IsRunning { get; private set; }
        public string Message { get; private set; }
        #endregion
    }
}
