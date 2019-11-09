using System.ComponentModel;
using System.Windows;
using GradProject.Views;
using GradProject.Models;

namespace GradProject.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для ShiftView.xaml
    /// </summary>
    public class ShiftViewModel : ViewModelBase //VM смены
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
        public IUser<User> CurrentUser //авторизованный пользователь
        {
            get { return _currentUser; }
            private set { _currentUser = value; OnPropertyChanged(); }
        }
        public decimal MoneyToAddOrWithdraw { get; set; } = (decimal)0.00; //деньки к внесению/изъятию
        public string EnteredLogin{ get; set; } //введенный при авторизации логин
        public string EnteredPassword { get; set; }//введенный при авторизации пароль
        public string UserName //ФИО пользователя
        {
            get { return _userName; }
            private set { _userName = value; OnPropertyChanged(); }
        }
        public Shift CurrentShift //текущая смены
        {
            get { return _currentShift; }
            private set { _currentShift = value; OnPropertyChanged(); }
        }
        #endregion
        #region Commands
        public RelayCommand AddMoney //внесение средств
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
                            MoneyToAddOrWithdraw = (decimal)0.00;
                        }
                    }));
            }
        }
        public RelayCommand WithdrawMoney //изъятие средств
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
                            MoneyToAddOrWithdraw = (decimal)0.00;
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
                                    db.SaveChanges();
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
                        OnWindowClosing(this);
                    }));
            }
        }
        #endregion
        #region Methods
        private void ShiftTransactionCompleted(object sender, ShiftTransactionEventArgs e) //обработчик события внесния/изъятия средств
        {
            if(!e.IsSuccessful)
                MessageBox.Show(e.Message);
        }
        public void OnWindowClosing(object sender, CancelEventArgs e = null) //обработчик события закрытия окна программы
        {
            if (CurrentShift.IsActive)
            {
                if (CurrentShift.EndShift())
                {
                    CurrentShift = new Shift();
                    CurrentUser = null;
                    UserName = null;
                    ButtonIsEnabled = false;
                }
            }
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
