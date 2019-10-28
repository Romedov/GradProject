using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Runtime.CompilerServices;
using System.Windows;
using GradProject.Models;

namespace GradProject
{
    public interface IUser<T> where T : User
    {
        T GetInstance();
    }
    public partial class User : INotifyPropertyChanged, IUser<User>
    {
        #region Fields
        string surName;
        string name;
        string fatherName;
        #endregion

        #region Properties
        [Key]
        [StringLength(50)]
        public string UId { get; private set; }

        [Required]
        [StringLength(50)]
        public string Password { get; private set; }

        [Required]
        [StringLength(50)]
        public string SurName
        {
            get { return surName; }
            private set
            {
                surName = value;
                OnPropertyChanged();
            }
        }

        [Required]
        [StringLength(50)]
        public string Name
        {
            get { return name; }
            private set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        [StringLength(50)]
        public string FatherName
        {
            get { return fatherName; }
            private set
            {
                fatherName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public static event EventHandler<SignInEventArgs> SigningIn;
        #endregion

        #region Methods
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public static User SignIn(string uid, string pwd)
        {
            using (CashboxDataContext db = new CashboxDataContext())
            {
                User user = null;
                try
                {
                    db.DBConnectionCheck();
                    user = db.Users.FirstOrDefault(u => uid == u.UId && pwd == u.Password);
                    if (user != null)
                    {
                        if (SigningIn != null)
                        {
                            SigningIn(user, new SignInEventArgs(true));
                            return user;
                        }
                    }
                    else
                    {
                        if (SigningIn != null)
                        {
                            SigningIn(user, new SignInEventArgs(false));
                            return user;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                return user;
            }
        }

        public User GetInstance()
        {
            return this;
        }
        #endregion
    }
}
