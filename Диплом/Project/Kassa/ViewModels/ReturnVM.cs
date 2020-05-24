using Kassa.Models;
using Kassa.Models.EventsArgs;
using Kassa.OtherComponents;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Kassa.ViewModels
{
    public class ReturnVM : ViewModelBase
    {
        #region Public

        public ReturnVM(IPageService pageService)
        {
            _pageService = pageService;
            SearchReceiptId = string.Empty;
            Receipt = new Receipt(ReceiptType.Return);
            Receipt.ReceiptPosted += OnPosted;
            SelectedItem = null;
            Quantity = string.Empty;

            SearchCommand = new Command(async () => await Search(), () => SearchReceiptId != string.Empty && SearchReceiptId != null && Receipt != null);
            SearchViaScanCommand = new Command(async () => await ScanAndSearch(), () => Receipt != null);
            RemoveCommand = new Command(() => RemoveItem(), () => Receipt != null && SelectedItem != null && Receipt.Items.Count > 0);
            ChangeQuantityCommand = new Command(() => ChangeQuantity(), () => _quantityRegex.IsMatch(Quantity) && SelectedItem != null);
            PostCommand = new Command(() => Post(), () => CanPost());


            App.ReturnVM = this;
        }

        public Receipt Receipt
        {
            get => _receipt;
            private set
            {
                _receipt = value ?? new Receipt(ReceiptType.Sale);
                OnPropertyChanged();
                SearchCommand?.ChangeCanExecute();
                SearchViaScanCommand?.ChangeCanExecute();
            }
        }

        public ReceiptItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                Quantity = value != null ? value.Quantity.ToString() : string.Empty;
                OnPropertyChanged();
                RemoveCommand?.ChangeCanExecute();
            }
        }

        public string SearchReceiptId
        {
            get => _searchReceiptId;
            set
            {
                _searchReceiptId = value ?? string.Empty;
                OnPropertyChanged();
                SearchCommand?.ChangeCanExecute();
            }
        }

        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = StringHelper.TruncateCommas(value, '.');
                OnPropertyChanged();
                ChangeQuantityCommand?.ChangeCanExecute();
            }
        }

        public Command PostCommand { get; }
        public Command RemoveCommand { get; }
        public Command SearchCommand { get; }
        public Command SearchViaScanCommand { get; }
        public Command ChangeQuantityCommand { get; }

        #endregion

        #region Private

        private readonly IPageService _pageService;
        private Receipt _receipt;
        private ReceiptItem _selectedItem;
        private string _searchReceiptId;
        private string _quantity;

        private readonly Regex _quantityRegex = new Regex(@"^\d+(\.\d\d?\d?)?$");

        private async Task Search()
        {
            try
            {
                using (var db = new CashRegisterContext())
                {
                    var receipt = db.GetReceipt(SearchReceiptId);
                    await _pageService.PushAsync(new Views.ReceiptView(receipt));
                    SearchReceiptId = string.Empty;
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
                    SearchReceiptId = result.Text;
                    await Search();
                });
            };
        }

        private void RemoveItem()
        {
            Receipt.RemoveItem(SelectedItem);
            RemoveCommand?.ChangeCanExecute();
            PostCommand?.ChangeCanExecute();
        }

        private void ChangeQuantity()
        {
            var q = float.Parse(Quantity.Replace('.', ','));
            if (q == 0)
            {
                RemoveItem();
            }
            else
            {
                SelectedItem.Quantity = q;
            }
            Receipt.ComputeTotalPrice();
        }

        private void Post()
        {
            Receipt.Post(CashRegisterContext.CurrentShift);
        }

        private async void OnPosted(object sender, ReceiptPostedEventArgs e)
        {
            if (e.Successful)
            {
                SelectedItem = null;
                Quantity = string.Empty;
                Receipt = new Receipt(ReceiptType.Return);
                Receipt.ReceiptPosted += OnPosted;
            }
            await _pageService.DisplayAlert("Уведомление", e.Message, "OK");
        }

        private bool CanPost()
        {
            return CashRegisterContext.CurrentShift != null && CashRegisterContext.CurrentShift.State == ShiftState.Running &&
                    Receipt.Items.Count > 0 && Receipt != null;
        }

        #endregion
    }
}
