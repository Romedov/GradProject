using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models
{
    public enum Roles
    {
        Regular=0,
        Admin=1,
    }
    public class User : IUser
    {
        #region Constructors
        public User() { }
        public User(string login, string password, string fname, string sname, string tname, Roles role, DateTime dt)
        {
            Login = login;
            Password = password;
            FirstName = fname;
            SecondName = sname;
            ThirdName = tname;
            Role = role;
            RegDateTime = dt;
        }
        #endregion
        #region Events
        #endregion
        #region Private props
        #endregion
        #region Public props
        public int ID { get; private set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public Roles Role { get; set; }
        public DateTime RegDateTime { get; set; }
        public IEnumerable<Shift> Shifts { get; set; }  //nav prop
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        #endregion
    }
}
