using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class ShiftFinishedEventArgs
    {
        #region Constructros
        public ShiftFinishedEventArgs(string message, bool isFinished = true)
        {
            this.Message = message;
            this.IsFinished = isFinished;
        }
        #endregion
        #region Public props
        public bool IsFinished { get; private set; }
        public string Message { get; private set; }
        #endregion
    }
}
