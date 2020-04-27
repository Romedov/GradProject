using Kassa.Models.EventsArgs;
using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kassa.Models
{
    public enum Roles
    {
        Regular=0,
        Admin=1,
    }
    public class User : IUser, IUserEditor
    {
        #region Constructors
        public User() 
        {
            Login = Password = FirstName = SecondName = ThirdName = string.Empty;
            Role = Roles.Regular;
        }
        #endregion
        #region Events
        public event EventHandler<UserChangesAppliedEventArgs> ChangesApplied;
        public event EventHandler<UserRegisteredEventArgs> Registered;
        #endregion
        #region Private props
        private string _password;
        #endregion
        #region Public props
        public int ID { get; private set; }
        public string Login { get; set; }
        public string Password 
        {
            get => this._password;
            set
            {
                if(value != null && value != string.Empty)
                {
                    this._password = SHA512ManagedHasher.GetHash(value, Encoding.UTF8);
                }
                else
                {
                    this._password = string.Empty;
                }
            }
        }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public Roles Role { get; set; }
        public DateTime RegDateTime { get; private set; }
        public IEnumerable<Shift> Shifts { get; set; }  //nav prop
        #endregion
        #region Private methods
        private bool IsEmpty()
        {
            string[] arr = { Login, Password, FirstName, SecondName, ThirdName};
            arr.ToList().ForEach(s =>
            {
                s = s.Replace(" ", "");
            });
            return arr.Any(e => e == string.Empty || e == null);
        }
        #endregion
        #region Public methods
        public static IUser SignIn(string login, string password)
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                if (ctx.CanConnect)
                {
                    return ctx.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                }
                else
                {
                    return null;
                }
            }
        }
        public void ApplyChanges()
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                var existingUser = ctx.Users.FirstOrDefault(u => u.Login == this.Login);
                if (existingUser != null && !IsEmpty())
                {
                    ctx.Users.Update(this);
                    ctx.SaveChanges();
                    ChangesApplied?.Invoke(this, new UserChangesAppliedEventArgs("Изменение сохранены!"));
                }
                else
                {
                    ChangesApplied?.Invoke(this, new UserChangesAppliedEventArgs("Не удалось сохранить изменения!", false));
                }
                
            }
        }

        public void Register()
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                var existingUser = ctx.Users.FirstOrDefault(u => u.Login == this.Login);
                if (existingUser == null && !IsEmpty())
                {
                    this.RegDateTime = DateTime.Now;
                    ctx.Users.Add(this);
                    ctx.SaveChanges();
                    Registered?.Invoke(this, new UserRegisteredEventArgs("Пользователь зарегистрирован!"));
                }
                else
                {
                    Registered?.Invoke(this, new UserRegisteredEventArgs("Не удалось зарегистрировать пользователя: " +
                        "не заполнено одно из полей, либо пользователь с таким логином уже существует!", false));
                }
            }
        }
        #endregion
    }
}
