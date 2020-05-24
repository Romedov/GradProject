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
    public class ItemVM : ViewModelBase
    {
        #region Public

        public ItemVM(IPageService pageService)
        {
            _pageService = pageService;
            Item = new Item();
            Price = Item.Price.ToString();
            RegisterCommand = new Command(async () => await Register(), () => NotEmpty() && Item != null && CheckPriceFormat());
            EditCommand = new Command(async () => await ApplyChanges(), () => NotEmpty() && Item != null && Item.ID != 0 && CheckPriceFormat());
            RemoveCommand = new Command(async () => await Remove(), () => Item != null && Item.ID != 0);
            SearchViaEntryCommand = new Command(async () => await Search(), () => SearchBarcode != string.Empty && SearchBarcode != null);
            SearchViaScanCommand = new Command(async () => await ScanAndSearch());
        }

        public IEditableItem Item 
        {
            get => _item;
            private set
            {
                _item = value ?? new Item();
                OnPropertyChanged();
                Barcode = _item.Barcode;
                Name = _item.Name;
                Price = _item.Price.ToString();
                Discount = _item.Discount.ToString();
                RegisterCommand?.ChangeCanExecute();
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
            }
        }

        public string Barcode 
        {
            get => Item?.Barcode;
            set
            {
                Item.Barcode = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
            }
        }

        public string Name
        {
            get => Item?.Name;
            set
            {
                Item.Name = value;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
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
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
            }
        }

        public string Discount
        {
            get => Item?.Discount.ToString();
            set
            {
                byte.TryParse(value, out byte n);
                Item.Discount = n;
                OnPropertyChanged();
                RegisterCommand?.ChangeCanExecute();
                EditCommand?.ChangeCanExecute();
                RemoveCommand?.ChangeCanExecute();
            }
        }

        public string SearchBarcode
        {
            get => _searchBarcode;
            set
            {
                _searchBarcode = value;
                OnPropertyChanged();
                SearchViaEntryCommand?.ChangeCanExecute();
            }
        }


        public Command RegisterCommand { get; }

        public Command EditCommand { get; }

        public Command RemoveCommand { get; }

        public Command SearchViaEntryCommand { get; }

        public Command SearchViaScanCommand { get; }

        #endregion

        #region Protected



        #endregion

        #region Private

        private IEditableItem _item;
        private string _price;
        private string _searchBarcode;
        private readonly IPageService _pageService;
        private readonly Regex _priceRegex = new Regex(@"^\d+(\.\d{2})?$");


        private async Task Register()
        {
            try
            {
                Item.Price = decimal.Parse(Price);
                using (var db = new CashRegisterContext())
                {
                    await db.AddItem(Item);
                }
                await _pageService.DisplayAlert("Уведомление", "Товар зарегистрирован!", "OK");
                Item = new Item();
            }
            catch (Exception ex)
            {
                await _pageService?.DisplayAlert("Уведомление", ex.Message, "OK");
            }
        }

        private async Task ApplyChanges()
        {
            try
            {
                Item.Price = decimal.Parse(Price);
                using (var db = new CashRegisterContext())
                {
                    await db.UpdateItem(Item);
                }
                await _pageService.DisplayAlert("Уведомление", "Изменения сохранены!", "OK");
            }
            catch (Exception ex)
            {
                await _pageService?.DisplayAlert("Уведомление", ex.Message, "OK");
            }
        }

        private async Task Remove()
        {
            try
            {
                using (var db = new CashRegisterContext())
                {
                    await db.RemoveItem(Item);
                }
                Item = new Item();
                await _pageService.DisplayAlert("Уведомление", "Изменения сохранены!", "OK");
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
                    await scan.Navigation.PopAsync();
                    SearchBarcode = result.Text;
                    await Search();
                });
            };
        }

        private bool NotEmpty()
        {
            return Barcode.Replace(" ", "") != string.Empty && Name.Trim() != string.Empty && Barcode != null && Name != null;
        }

        private bool CheckPriceFormat()
        {
            return _priceRegex.IsMatch(Price);
        }

        #endregion
    }
}
