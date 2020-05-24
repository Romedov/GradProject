using Kassa.Models.EventsArgs;
using System;

namespace Kassa.Models.Interfaces
{
    public interface IUserEditor
    {
        event EventHandler<UserRegisteredEventArgs> Registered;
        int ID { get; }
        string Login { get; set; }
        string Password { get; set; }
        string FirstName { get; set; }
        string SecondName { get; set; }
        string ThirdName { get; set; }
        Roles Role { get; set; }
        void Register();
    }
}
