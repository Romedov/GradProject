using Kassa.Models;
using Kassa.Models.Interfaces;
using Kassa.OtherComponents;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Kassa.ViewModels
{
    public class DisposalVM : ViewModelBase
    {
        #region Public 

        public DisposalVM(IPageService pageservice)
        {
            _pageService = pageservice;
            RegisterCommand = new Command(async () => await Register(), () => NotEmpty() && CheckQuantityFormat());
            SearchCommand = new Command(async () => await Search(), () => SearchBarcode != string.Empty && SearchBarcode != null);
            SearchViaScanCommand = new Command(async () => await ScanAndSearch());
            Basis = string.Empty;
            Quantity = "0";
        }

        public ISellableItem Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }

        public IDisposal Disposal
        {
            get => _disposal;
            set
            {
                _disposal = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }

        public string Basis
        {
            get => _basis;
            set
            {
                _basis = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }

        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = StringHelper.TruncateCommas(value, '.');
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
            }
        }

        public string SearchBarcode
        {
            get => _searchBarcode;
            set
            {
                _searchBarcode = value;
                OnPropertyChanged();
                SearchCommand?.ChangeCanExecute();
            }
        }

        public Command RegisterCommand { get; }

        public Command SearchCommand { get; }

        public Command SearchViaScanCommand { get; }

        #endregion

        #region Protected



        #endregion

        #region Private
        private string _basis;
        private string _quantity;
        private string _searchBarcode;
        private ISellableItem _item;
        private IDisposal _disposal;
        private readonly IPageService _pageService;
        private readonly Regex _quantityRegex = new Regex(@"^\d+(\.\d\d?\d?)?$");

        private async Task Register()
        {
            try
            {
                Disposal.Quantity = float.Parse(Quantity.Replace('.', ','));
                Disposal.Basis = Basis;
                await Disposal.Register(Item);
                await _pageService.DisplayAlert("Уведомление", "Списание произведено!", "OK");
                Basis = string.Empty;
                Quantity = "0";
                SearchBarcode = string.Empty;
                Disposal = null;
                Item = null;
            }
            catch (Exception ex)
            {
                await _pageService?.DisplayAlert("Уведомление", ex.Message, "OK");
            }
        }

        private async Task Search()
        {
            try
            {
                using (var db = new CashRegisterContext())
                {
                    var item = db.GetItem(SearchBarcode);
                    Item = item;
                }
                Disposal = new Disposal(CashRegisterContext.CurrentShift, Item);
            }
            catch (Exception ex)
            {
                await _pageService?.DisplayAlert("Уведомление", ex.Message, "OK");
            }
        }

        private async Task ScanAndSearch()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                var request = await Permissions.RequestAsync<Permissions.Camera>();
                if (request != PermissionStatus.Granted)
                {
                    return;
                }
            }
            var scan = new ZXingScannerPage();
            await _pageService.PushAsync(scan);
            scan.OnScanResult += result =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _pageService.Page.Navigation.PopAsync();
                    SearchBarcode = result.Text;
                    await Search();
                });
            };
        }

        private bool NotEmpty()
        {
            if (float.TryParse(Quantity.Replace('.', ','), out float n))
            {
                return Disposal != null && Item != null && Item.ID != 0 && Basis != null &&
                    Basis != string.Empty && Quantity != null && Quantity != string.Empty && n > (float)0;
            }
            else return false;
            
        }

        private bool CheckQuantityFormat()
        {
            if (_quantityRegex.IsMatch(Quantity))
            {
                if (float.Parse(Quantity.Replace('.', ',')) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
