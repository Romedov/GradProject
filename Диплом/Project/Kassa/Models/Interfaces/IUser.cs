using System;

namespace Kassa.Models.Interfaces
{
    public interface IUser
    {
        int ID { get; }
        string Login { get; }
        string FirstName { get; }
        string SecondName { get; }
        string ThirdName { get; }
        Roles Role { get; }
        DateTime RegDateTime { get; }
    }
}
