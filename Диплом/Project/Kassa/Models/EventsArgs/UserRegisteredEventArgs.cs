using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.EventsArgs
{
    public class UserRegisteredEventArgs : EventArgsBase
    {
        public UserRegisteredEventArgs(string message, bool successful = true) : base(message, successful)
        {

        }
    }
}