using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class ShiftFinishedEventArgs : EventArgsBase
    {
        public ShiftFinishedEventArgs(string message, bool successful = true) : base(message, successful)
        {
            
        }
    }
}
