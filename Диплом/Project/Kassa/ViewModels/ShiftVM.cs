using Kassa.Models;
using Kassa.Models.Interfaces;
using Kassa.OtherComponents;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kassa.ViewModels
{
    enum Operation
    {
        Add,
        Withdraw,
    }
    public class ShiftVM : ViewModelBase
    {
        #region Public

        public ShiftVM(IPageService pageService)
        {
            _pageService = pageService;
            SideMoney = string.Empty;
            Shift = CashRegisterContext.CurrentShift ?? new Shift();
            #region Commands
            StartCommand = new Command(async () => await Start(), () => Shift.State == ShiftState.Created);
            FinishCommand = new Command(async () => await Finish(), () => Shift.State == ShiftState.Running);
            AddCommand = new Command(() => InitializeAdd(), () => Shift.State == ShiftState.Running);
            WithdrawCommand = new Command(() => InitializeWithdraw(), () => Shift.State == ShiftState.Running && Shift.Balance > 0);
            OperationCommand = new Command(async () => await InitializeOperation(), 
                                            () => Shift.State == ShiftState.Running && SideMoney != string.Empty && SideMoney != null && CheckMoneyFormat());
            HideKeyboardCommand = new Command(() => HideKeyboard());
            BackSpaceCommand = new Command(() => DoBackSpace(), () => SideMoney != string.Empty && SideMoney != null);
            ClearCommand = new Command(() => Clear(), () => SideMoney != string.Empty && SideMoney != null);
            #endregion
        }

        public IShift Shift 
        {
            get => CashRegisterContext.CurrentShift;
            set
            {
                CashRegisterContext.CurrentShift = value as Shift ?? new Shift();
                StartCommand?.ChangeCanExecute();
                FinishCommand?.ChangeCanExecute();
                AddCommand?.ChangeCanExecute();
                WithdrawCommand?.ChangeCanExecute();
                OnPropertyChanged();
            }
        }

        public string SideMoney
        {
            get => _sideMoney;
            set 
            {
                _sideMoney = StringHelper.TruncateCommas(value, '.');
                OnPropertyChanged();

                OperationCommand?.ChangeCanExecute();
                BackSpaceCommand?.ChangeCanExecute();
                ClearCommand?.ChangeCanExecute();
            }
        }

        public bool KeyboardShown 
        {
            get => _keyboardShown;
            set
            {
                _keyboardShown = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        public Command StartCommand { get; }
        public Command FinishCommand { get; }
        public Command AddCommand { get; }
        public Command WithdrawCommand { get; }
        public Command OperationCommand { get; }
        public Command HideKeyboardCommand { get; }
        public Command BackSpaceCommand { get; }
        public Command ClearCommand { get; }

        #endregion

        #endregion

        #region Private
        #region Fields
        private Operation _operation;
        private string _sideMoney;
        private bool _keyboardShown;
        private readonly IPageService _pageService;
        private readonly Regex _moneyRegex = new Regex(@"^\d+(\.\d{2})?$");
        #endregion

        #region Methods
        private async Task Start()
        {
            try
            {
                using (var db = new CashRegisterContext())
                {
                    Shift = db.GetLastUnfinishedShift(CashRegisterContext.CurrentUser) ?? new Shift();
                }

                if (Shift.ID == 0)
                {
                    await Shift.Start(CashRegisterContext.CurrentUser);
                }
                #region Commands
                StartCommand?.ChangeCanExecute();
                FinishCommand?.ChangeCanExecute();
                AddCommand?.ChangeCanExecute();
                WithdrawCommand?.ChangeCanExecute();
                #endregion
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Уведомление", ex.Message, "OK");
            }
        }

        private async Task Finish()
        {
            try
            {
                await Shift.Finish();
                Shift = new Shift();
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Уведомление", ex.Message, "OK");
            }
        }

        private void InitializeAdd()
        {
            _operation = Operation.Add;
            KeyboardShown = true;
        }

        private void InitializeWithdraw()
        {
            _operation = Operation.Withdraw;
            KeyboardShown = true;
        }

        private async Task InitializeOperation()
        {
            if (_operation == Operation.Add)
            {
                await Add();
            }
            else
            {
                await Withdraw();
            }
        }

        private async Task Add()
        {
            try
            {
                await Shift.AddMoney(decimal.Parse(SideMoney.Replace('.', ',')));
                WithdrawCommand?.ChangeCanExecute();
                KeyboardShown = false;
                SideMoney = string.Empty;
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Уведомление", ex.Message, "OK");
            }
        }

        private async Task Withdraw()
        {
            try
            {
                await Shift.WithdrawMoney(decimal.Parse(SideMoney.Replace('.', ',')));
                WithdrawCommand?.ChangeCanExecute();
                KeyboardShown = false;
                SideMoney = string.Empty;
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Уведомление", ex.Message, "OK");
            }
        }

        private bool CheckMoneyFormat()
        {
            return _moneyRegex.IsMatch(SideMoney);
        }

        private void HideKeyboard()
        {
            KeyboardShown = false;
        }

        private void DoBackSpace()
        {
            SideMoney = SideMoney.Remove(SideMoney.Length - 1);
        }

        private void Clear()
        {
            SideMoney = string.Empty;
        }
        #endregion
        #endregion
    }
}
