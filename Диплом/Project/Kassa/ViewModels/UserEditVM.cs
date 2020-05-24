using Kassa.Models;
using Kassa.Models.Interfaces;
using Kassa.OtherComponents;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kassa.ViewModels
{
    public class UserEditVM : ViewModelBase //completed
    {
        #region Public

        public UserEditVM(IPageService pageService)
        {
            Initialize();
            _pageService = pageService;
            EditCommand = new Command(async () => await Edit(), () => NotEmpty() && User != null);
            SearchCommand = new Command(async () => await Search(), () => SearchLogin.Replace(" ", "") != string.Empty);
            RemoveCommand = new Command(async () => await Remove(), () => NotEmpty() && User != null);
        }

        public bool ButtonsEnabled
        {
            get => _buttonsEnabled;
            set
            {
                _buttonsEnabled = value;
                OnPropertyChanged();
            }
        }
        public IUserEditor User
        {
            get => _user;
            set
            {
                _user = value;
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
                OnPropertyChanged();
            }
        }
        public string SearchLogin
        {
            get => _searchLogin;
            set
            {
                _searchLogin = value;
                OnPropertyChanged();
                SearchCommand?.ChangeCanExecute();
            }
        }
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
                OnPropertyChanged();
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
                OnPropertyChanged();
            }
        }
        public string SecondName
        {
            get => _secondName;
            set
            {
                _secondName = value;
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
                OnPropertyChanged();
            }
        }
        public string ThirdName
        {
            get => _thirdName;
            set
            {
                _thirdName = value;
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
                OnPropertyChanged();
            }
        }
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
                OnPropertyChanged();
            }
        }

        public Command EditCommand { get; }
        public Command SearchCommand { get; }
        public Command RemoveCommand { get; }

        #endregion

        #region Protected



        #endregion

        #region Private 
        #region Fields
        private IUserEditor _user;
        private string _searchLogin;
        private string _login;
        private string _password;
        private string _firstName;
        private string _secondName;
        private string _thirdName;
        private bool _isAdmin;
        private readonly IPageService _pageService;
        private bool _buttonsEnabled;
        #endregion
        private void Initialize() //completed
        {
            User = CashRegisterContext.CurrentUser;
            Login = User.Login;
            Password = string.Empty;
            FirstName = User.FirstName;
            SecondName = User.SecondName;
            ThirdName = User.ThirdName;
            IsAdmin = User.Role == Roles.Admin;
            SearchLogin = string.Empty;
            ButtonsEnabled = false;
        }

        private bool NotEmpty()
        {
            string[] props = { Login, FirstName, SecondName, ThirdName };
            props.ToList().ForEach(prop => prop = prop.Replace(" ", ""));
            return props.All(prop => prop != string.Empty && prop != null);
        } //comleted

        private async Task Edit() //completed
        {
            AssignData();

            using (var db = new CashRegisterContext())
            {
                if (await db.UpdateUser(User as User))
                {
                    await _pageService.DisplayAlert("Уведомление", "Изменения сохранены!", "ОК");
                }
                else
                {
                    await _pageService.DisplayAlert("Уведомление", "Не удалось сохранить изменения!", "ОК");
                }
            };
        }

        private async Task Remove() //completed
        {
            if (CashRegisterContext.CurrentUser.ID == User.ID)
            {
                await _pageService.DisplayAlert("Уведомление", "Нельзя удалить текущего пользователя!", "ОК");
                return;
            }

            using (var db = new CashRegisterContext())
            {
                if (await db.RemoveUser(User as User))
                {
                    await _pageService.DisplayAlert("Уведомление", "Пользователь удален!", "ОК");
                    ClearData();
                    RemoveCommand.ChangeCanExecute();
                }
                else
                {
                    await _pageService.DisplayAlert("Уведомление", "Не удалось удалить пользователя!", "ОК");
                }
            };
        }

        private async Task Search() //completed
        {
            using (var db = new CashRegisterContext())
            {
                User = db.GetUserByLogin(SearchLogin);
                if (User != null)
                {
                    DisplayData();
                    ButtonsEnabled = true;
                }
                else
                {
                    await _pageService.DisplayAlert("Уведомление", "Нет данных о пользователе!", "ОК");
                }
            };
        }

        private void AssignData()
        {
            User.Login = Login;
            User.FirstName = FirstName;
            User.SecondName = SecondName;
            User.ThirdName = ThirdName;
            User.Role = IsAdmin ? Roles.Admin : Roles.Regular;
            if (Password != string.Empty)
            {
                User.Password = Password;
            }
        }

        private void ClearData()
        {
            User = null;
            Login = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            SecondName = string.Empty;
            ThirdName = string.Empty;
            IsAdmin = false;
        }

        private void DisplayData()
        {
            SearchLogin = string.Empty;
            Login = User.Login;
            FirstName = User.FirstName;
            SecondName = User.SecondName;
            ThirdName = User.ThirdName;
            IsAdmin = User.Role == Roles.Admin;
        }

        #endregion
    }
}
