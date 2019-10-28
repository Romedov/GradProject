using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GradProject.Views;
using GradProject.Models;

namespace GradProject.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для ShiftView.xaml
    /// </summary>
    public class ShiftViewModel : ViewModelBase
    {
        #region Constructors
        public ShiftViewModel()
        {
            User.SigningIn += UserSigningIn;
            CurrentShift = new Shift();
            Shift.TransactionCompleted += ShiftTransactionCompleted;
        }
        #endregion
        #region Events
        #endregion
        #region Fields
        private Window _wnd;
        private bool _buttonIsEnabled = false;
        private RelayCommand _relayCommand = null;
        private IUser<User> _currentUser;
        private Shift _currentShift;
        private string _userName;
        #endregion
        #region Properties
        public bool ButtonIsEnabled //свойство активации кнопок
        {
            get { return _buttonIsEnabled; }
            private set
            {
                if (_buttonIsEnabled != value)
                {
                    _buttonIsEnabled = value; OnPropertyChanged();
                }
            }
        }
        public IUser<User> CurrentUser
        {
            get { return _currentUser; }
            private set { _currentUser = value; OnPropertyChanged(); }
        }
        public decimal MoneyToAddOrWithdraw{ get; set; }
        public string EnteredLogin{ get; set; } //введенный при авторизации логин
        public string EnteredPassword { get; set; }//введенный при авторизации пароль
        public string UserName
        {
            get { return _userName; }
            private set { _userName = value; OnPropertyChanged(); }
        }
        public Shift CurrentShift
        {
            get { return _currentShift; }
            private set { _currentShift = value; OnPropertyChanged(); }
        }
        #endregion
        #region Commands
        public RelayCommand AddMoney
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        _wnd = null;
                        _wnd = new AddShiftWindow();
                        if(_wnd.ShowDialog() == true)
                        {
                            Shift.AddMoneyAsync(MoneyToAddOrWithdraw, CurrentShift);
                        }
                    }));
            }
        }
        public RelayCommand WithdrawMoney
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        _wnd = null;
                        _wnd = new WithdrawShiftWindow();
                        if (_wnd.ShowDialog() == true)
                        {
                            Shift.WithdrawMoneyAsync(MoneyToAddOrWithdraw, CurrentShift);
                        }
                    }));
            }
        }
        public RelayCommand AuthWndOpenCommand //команда запуска окна авторизации
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        _wnd = null;
                        _wnd = new AuthorizationWindow();
                        _wnd.ShowDialog();
                    }));
            }
        }
        public RelayCommand AuthorizationCommand //команда, осуществляющая логику авторизации
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        if (_wnd.DialogResult == true)
                        {
                            CurrentUser = null;
                            CurrentUser = User.SignIn(EnteredLogin, EnteredPassword);
                            if (CurrentUser != null)
                            {
                                CurrentShift = null;
                                CurrentShift = Shift.ShiftStart(CurrentUser);
                                UserName = string.Format("{0} {1}. {2}.",
                                        _currentUser.GetInstance().SurName,
                                        _currentUser.GetInstance().Name[0],
                                        _currentUser.GetInstance().FatherName[0]);
                                ButtonIsEnabled = true;
                                using(CashboxDataContext db = new CashboxDataContext())
                                {
                                    db.FreeItems.Add(new FreeItem(CurrentShift.SId));
                                }
                            }
                        }
                    }));
            }
        }
        public RelayCommand EndShift //команда запуска окна авторизации
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        if (CurrentShift.EndShift())
                        {
                            CurrentShift = new Shift();
                            CurrentUser = null;
                            UserName = null;
                            ButtonIsEnabled = false;
                        }
                    }));
            }
        }
        #endregion
        #region Methods
        private void ShiftTransactionCompleted(object sender, ShiftTransactionEventArgs e)
        {
            if(!e.IsSuccessful)
                MessageBox.Show(e.Message);
        }
        private void UserSigningIn(object sender, SignInEventArgs e) //обработчик события авторизации пользователя
        {
            if (!e.IsSuccessful)
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
        #endregion
    }
}
