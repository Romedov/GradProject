using Kassa.Models.EventsArgs;
using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kassa.Models
{
    public enum Roles
    {
        Regular=0,
        Admin=1,
    }
    public class User : IUser, IUserEditor, INotifyPropertyChanged
    {
        #region Constructors
        public User() 
        {
            Login = Password = FirstName = SecondName = ThirdName = string.Empty;
            Role = Roles.Regular;
        }
        #endregion
        #region Events
        public event EventHandler<UserRegisteredEventArgs> Registered;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Private props
        private string _login;
        private string _password;
        private string _firstName;
        private string _secondName;
        private string _thirdName;
        private Roles _role;
        #endregion
        #region Public props
        public int ID { get; private set; }
        public string Login 
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }
        public string Password 
        {
            get => _password;
            set
            {
                if(value != null && value != string.Empty)
                {
                    _password = value;
                }
                else
                {
                    _password = string.Empty;
                }
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }
        public string SecondName
        {
            get => _secondName;
            set
            {
                _secondName = value;
                OnPropertyChanged();
            }
        }
        public string ThirdName
        {
            get => _thirdName;
            set
            {
                _thirdName = value;
                OnPropertyChanged();
            }
        }
        public Roles Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged();
            }
        }
        public DateTime RegDateTime { get; private set; }
        public IList<Shift> Shifts { get; set; }  //nav prop
        #endregion
        #region Protected methods
        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
        #region Public methods
        public void Register()
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                if (!ctx.CanConnect)
                {
                    Registered?.Invoke(this, new UserRegisteredEventArgs("Нет соединения с базой данных!", false));
                    return;
                }
                
                var existingUser = ctx.Users.FirstOrDefault(u => u.Login == this.Login);
                if (existingUser == null)
                {
                    this.RegDateTime = DateTime.Now;
                    this.Password = OtherComponents.SHA512ManagedHasher.GetHash(Password, Encoding.UTF8);
                    ctx.Users.Add(this);
                    ctx.SaveChanges();
                    Registered?.Invoke(this, new UserRegisteredEventArgs("Пользователь зарегистрирован!"));
                }
                else
                {
                    Registered?.Invoke(this, new UserRegisteredEventArgs("Не удалось зарегистрировать пользователя: " +
                        "пользователь с таким логином уже существует!", false));
                }
            }
        }
        #endregion
    }
}
