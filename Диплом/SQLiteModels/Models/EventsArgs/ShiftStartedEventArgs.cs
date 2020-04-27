using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class ShiftStartedEventArgs : EventArgsBase
    {
        public ShiftStartedEventArgs(string message, bool successful = true) : base(message, successful)
        {
            
        }
    }
}
