using Kassa.Models.EventsArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.Interfaces
{
    interface IUserEditor
    {
        event EventHandler<UserChangesAppliedEventArgs> ChangesApplied;
        event EventHandler<UserRegisteredEventArgs> Registered;
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public Roles Role { get; set; }
        void Register();
        void ApplyChanges();
    }
}
