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
    public class SupplyVM : ViewModelBase
    {
        #region Public

        public SupplyVM(IPageService pageservice)
        {
            _pageService = pageservice;
            RegisterCommand = new Command(async () => await Register(), () => NotEmpty() && CheckPriceFormat() && CheckQuantityFormat());
            SearchCommand = new Command(async () => await Search(), () => SearchBarcode != string.Empty && SearchBarcode != null);
            SearchViaScanCommand = new Command(async () => await ScanAndSearch());
            Basis = string.Empty;
            Quantity = "0";
            Price = "0";
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

        public ISupply Supply
        {
            get => _supply;
            set
            {
                _supply = value;
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
                if (decimal.TryParse(Price?.Replace('.', ','), out decimal price) && float.TryParse(value?.Replace('.', ','), out float quantity))
                {
                    TotalPrice = price * Convert.ToDecimal(quantity);
                }
            }
        }

        public string Price
        {
            get => _price;
            set
            {
                _price = StringHelper.TruncateCommas(value, '.');
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
                if (decimal.TryParse(value?.Replace('.', ','), out decimal price) && float.TryParse(Quantity?.Replace('.', ','), out float quantity))
                {
                    TotalPrice = price * Convert.ToDecimal(quantity);
                }
            }
        }

        public decimal TotalPrice 
        { 
            get => _totalPrice;
            private set
            {
                _totalPrice = value;
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
        private string _price;
        private string _quantity;
        private decimal _totalPrice;
        private string _searchBarcode;
        private ISellableItem _item;
        private ISupply _supply;
        private readonly IPageService _pageService;
        private readonly Regex _quantityRegex = new Regex(@"^\d+(\.\d\d?\d?)?$");
        private readonly Regex _priceRegex = new Regex(@"^\d+(\.\d{2})?$");

        private async Task Register()
        {
            try
            {
                Supply.Quantity = float.Parse(Quantity?.Replace('.', ','));
                Supply.Price = decimal.Parse(Price?.Replace('.', ','));
                Supply.Basis = Basis;
                await Supply.Register(Item as Item);
                await _pageService.DisplayAlert("Уведомление", "Приемка пороизведена!", "OK");
                Basis = string.Empty;
                Quantity = "0";
                Price = "0";
                SearchBarcode = string.Empty;
                Supply = null;
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
                Supply = new Supply(CashRegisterContext.CurrentShift, Item);
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
            return Supply != null && Item != null && Item.ID != 0 && Basis != null && 
                Basis != string.Empty && Quantity != null && Quantity != string.Empty && TotalPrice > 0;
        }

        private bool CheckPriceFormat()
        {
            if (_priceRegex.IsMatch(Price))
            {
                if (decimal.Parse(Price?.Replace('.', ',')) > 0)
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
        private bool CheckQuantityFormat()
        {
            if (_quantityRegex.IsMatch(Quantity))
            {
                if (float.Parse(Quantity?.Replace('.', ',')) > 0)
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
