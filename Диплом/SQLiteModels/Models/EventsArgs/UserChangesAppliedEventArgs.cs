using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class UserChangesAppliedEventArgs : EventArgsBase
    {
        public UserChangesAppliedEventArgs(string message, bool successful = true) : base(message, successful)
        {
            
        }
    }
}
