using System;

namespace Kassa.Models.EventsArgs
{
    public class EventArgsBase : EventArgs
    {
        public EventArgsBase(string message, bool successful = true)
        {
            this.Message = message;
            this.Successful = successful;
        }
        
        public string Message { get; }
        public bool Successful { get; }
    }
}