using Kassa.Models;
using Kassa.Models.EventsArgs;
using Kassa.Models.Interfaces;
using Kassa.OtherComponents;
using System.Linq;
using Xamarin.Forms;

namespace Kassa.ViewModels
{
    public class UserRegisterVM : ViewModelBase
    {
        #region Public

        public UserRegisterVM(IPageService pageService)
        {
            _pageService = pageService;
            Initialize();
        }

        public IUserEditor User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }
        public string Login
        {
            get => User.Login;
            set
            {
                User.Login = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }
        public string FirstName 
        {
            get => User.FirstName;
            set
            {
                User.FirstName = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }
        public string SecondName 
        {
            get => User.SecondName;
            set
            {
                User.SecondName = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }
        public string ThirdName 
        {
            get => User.ThirdName;
            set
            {
                User.ThirdName = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }
        public bool IsAdmin 
        {
            get => User.Role == Roles.Admin ? true : false;
            set
            {
                User.Role = value == false ? Roles.Regular : Roles.Admin;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            } 
        }
        public Command RegisterCommand { get; private set; }

        #endregion

        #region Protected



        #endregion

        #region Private 

        private IUserEditor _user;
        private string _password;
        private readonly IPageService _pageService;

        private void Initialize()
        {

            Password = string.Empty;
            User = new User();
            Login = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            SecondName = string.Empty;
            ThirdName = string.Empty;
            User.Registered += OnUserRegistered;
            RegisterCommand = new Command(() => Register(), () => NotEmpty());
        }
        private bool NotEmpty()
        {
            string[] props = { Login, Password, FirstName, SecondName, ThirdName };
            props.ToList().ForEach(s =>
            {
                s = s.Replace(" ", "");
            });
            return props.All(e => e != string.Empty && e != null);
        }

        private async void OnUserRegistered(object sender, UserRegisteredEventArgs e)
        {
            await _pageService.DisplayAlert("Уведомление", e.Message, "OK");
            if (e.Successful)
            {
                User = new User();
            }
        }

        private void Register()
        {
            User.Password = Password;
            User.Register();
        }

        #endregion
    }
}
