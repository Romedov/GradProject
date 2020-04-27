using System;

namespace Kassa.Models.EventsArgs
{
    public class SupplyRegisteredEventArgs : EventArgsBase
    {
        public SupplyRegisteredEventArgs(string message, bool successful = true)  : base(message, successful)
        {
            
        }
    }
}