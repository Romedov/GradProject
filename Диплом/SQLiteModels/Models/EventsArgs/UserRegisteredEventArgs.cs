using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class UserRegisteredEventArgs
    {
        public UserRegisteredEventArgs(string message, bool successful = true)
        {
            this.Message = message;
            this.Successful = successful;
        }
        public string Message { get; private set; }
        public bool Successful { get; private set; }
    }
}
