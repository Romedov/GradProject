using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCoreTest.Models.Interfaces
{
    public interface IUser
    {
        public int ID { get; }
        public string Login { get; }
        public string FirstName { get; }
        public string SecondName { get; }
        public string ThirdName { get; }
        public Roles Role { get; }
        public DateTime RegDateTime { get; }
    }
}
