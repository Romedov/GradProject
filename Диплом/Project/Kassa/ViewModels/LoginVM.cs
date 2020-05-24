using System;
using System.Text;
using System.Threading.Tasks;
using Kassa.Models;
using Kassa.OtherComponents;
using Xamarin.Forms;

namespace Kassa.ViewModels
{
    public class LoginVM : ViewModelBase
    {
        #region Public
        public LoginVM(IPageService pageService)
        {
            _pageService = pageService;
            Login = string.Empty;
            Password = string.Empty;
            MyCommand = new Command(async () => await SignIn(), () => NotEmpty());
        }


        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
                MyCommand?.ChangeCanExecute();
            }
        }
        public string Password 
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                MyCommand?.ChangeCanExecute();
            }
        }

        public Command MyCommand { get; }

        #endregion

        #region Private
        private readonly IPageService _pageService;
        private string _login;
        private string _password;

        private bool NotEmpty()
        {
            return Login.Replace(" ", "") != string.Empty && Password.Replace(" ", "") != string.Empty;
        }

        private async Task SignIn()
        {
            try
            {
                using (var db = new CashRegisterContext())
                {
                    db.SignIn(Login, SHA512ManagedHasher.GetHash(Password, Encoding.UTF8));
                    await _pageService.PushModalAsync(new Views.MainPage());
                    Login = string.Empty;
                    Password = string.Empty;
                }
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Уведомление", ex.Message, "ОК");
            }
        }
        #endregion
    }
}
