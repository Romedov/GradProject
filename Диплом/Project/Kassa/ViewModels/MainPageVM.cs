using Kassa.Models;
using Kassa.OtherComponents;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kassa.ViewModels
{
    public class MainPageVM : ViewModelBase
    {
        #region Public

        public MainPageVM(IPageService pageService)
        {
            IsShiftRunning = false;
            ShiftStateNotifier.MainPageVM = this;
            _pageService = pageService;
            ExitCommand = new Command(async () => await Exit());
        }

        public bool IsShiftRunning
        {
            get => _isShiftRunning;
            private set
            {
                _isShiftRunning = value;
                OnPropertyChanged();
            }
        }

        public Command ExitCommand { get; }

        public void OnStateChanged(ShiftState state)
        {
            if (state == ShiftState.Running)
            {
                IsShiftRunning = true;
            }
            else
            {
                IsShiftRunning = false;
            }
        }

        #endregion

        #region Private
        private bool _isShiftRunning;
        private readonly IPageService _pageService;

        private async Task Exit()
        {
            try
            {
                using (var db = new CashRegisterContext())
                {
                    if (CashRegisterContext.CurrentShift.State == ShiftState.Running)
                    {
                        await db.UpdateShift(CashRegisterContext.CurrentShift);
                    }
                    await _pageService.Page.Navigation.PopModalAsync();
                    CashRegisterContext.CurrentShift = null;
                }
            }
            catch (Exception ex)
            {
                var result = _pageService.DisplayAlert("Уведомление", $"Не удалось сохранить статистику смены! Вы действительно хотите выйти?\nПричина: {ex.Message}", "Да", "Нет").Result;
                if (result)
                {
                    await _pageService.Page.Navigation.PopModalAsync();
                    CashRegisterContext.CurrentShift = null;
                }
            }
        }
        #endregion
    }
}
