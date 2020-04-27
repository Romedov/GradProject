using System;

namespace Kassa.Models.EventsArgs
{
    public class ItemChangesAppliedEventArgs : EventArgsBase
    {
        public ItemChangesAppliedEventArgs(string message, bool successful = true) : base(message, successful)
        {
            
        }
    }
}