using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class MoneyWithdrawnEventArgs : EventArgsBase
    {
        public MoneyWithdrawnEventArgs(string message, bool successful = true)  : base(message, successful)
        {
            
        }
    }
}
