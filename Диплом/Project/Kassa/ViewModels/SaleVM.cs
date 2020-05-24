using Kassa.Models;
using Kassa.Models.EventsArgs;
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
    public class SaleVM : ViewModelBase
    {
        #region Public

        public SaleVM(IPageService pageService)
        {
            _pageService = pageService;
            Receipt = new Receipt(ReceiptType.Sale);
            Receipt.ReceiptPosted += OnPosted;
            SelectedItem = null;
            SearchBarcode = string.Empty;
            Quantity = string.Empty;
            KeyboardShown = false;
            Payment = string.Empty;

            SearchCommand = new Command(async () => await Search(), () => SearchBarcode != string.Empty && SearchBarcode != null && Receipt != null);
            SearchViaScanCommand = new Command(async () => await ScanAndSearch(), () => Receipt != null);
            RemoveCommand = new Command(() => RemoveItem(), () => Receipt != null && SelectedItem != null && Receipt.Items.Count > 0);
            ChangeQuantityCommand = new Command(() => ChangeQuantity(), () => _quantityRegex.IsMatch(Quantity) && SelectedItem != null);

            KeyboardVisualizeCommand = new Command(() => KeyboardVisualize(), () => Receipt.Items.Count > 0);
            PostCommand = new Command(() => Post(), () => CanPost());
            BackSpaceCommand = new Command(() => DoBackSpace(), () => Payment != string.Empty && Payment != null);
            ClearCommand = new Command(() => Clear(), () => Payment != string.Empty && Payment != null);
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

        public string SearchBarcode 
        {
            get => _searchBarcode;
            set
            {
                _searchBarcode = value ?? string.Empty;
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

        public string Payment
        {
            get => _payment;
            set
            {
                _payment = StringHelper.TruncateCommas(value, '.');
                OnPropertyChanged();
                BackSpaceCommand?.ChangeCanExecute();
                ClearCommand?.ChangeCanExecute();
                PostCommand?.ChangeCanExecute();
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

        public Command PostCommand { get; }
        public Command BackSpaceCommand { get; }
        public Command ClearCommand { get; }
        public Command KeyboardVisualizeCommand { get; }
        public Command RemoveCommand { get; }
        public Command SearchCommand { get; }
        public Command SearchViaScanCommand { get; }
        public Command ChangeQuantityCommand { get; }
        
        #endregion

        #region Private

        private Receipt _receipt;
        private ReceiptItem _selectedItem;
        private readonly IPageService _pageService;
        private string _searchBarcode;
        private string _quantity;
        private string _payment;
        private bool _keyboardShown;
        private readonly Regex _quantityRegex = new Regex(@"^\d+(\.\d\d?\d?)?$");
        private readonly Regex _paymentRegex = new Regex(@"^\d+(\.\d{2})?$");

        private async Task Search()
        {
            try
            {
                using (var db = new CashRegisterContext())
                {
                    var item = db.GetItem(SearchBarcode);
                    AddItem(item);
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

        private void AddItem(ISellableItem item)
        {
            Receipt.AddItem(item);
            SearchBarcode = string.Empty;
            RemoveCommand?.ChangeCanExecute();
            PostCommand?.ChangeCanExecute();
            KeyboardVisualizeCommand?.ChangeCanExecute();
        }

        private void RemoveItem()
        {
            Receipt.RemoveItem(SelectedItem);
            RemoveCommand?.ChangeCanExecute();
            PostCommand?.ChangeCanExecute();
            KeyboardVisualizeCommand?.ChangeCanExecute();
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

        private void DoBackSpace()
        {
            Payment = Payment.Remove(Payment.Length - 1);
        }

        private void Clear()
        {
            Payment = string.Empty;
        }

        private void KeyboardVisualize()
        {
            KeyboardShown = KeyboardShown ? false : true;
            if (!KeyboardShown)
            {
                Payment = string.Empty;
            }
        }

        private void Post()
        {
            Receipt.Post(CashRegisterContext.CurrentShift, decimal.Parse(Payment.Replace('.',',')));
        }

        private async void OnPosted(object sender, ReceiptPostedEventArgs e)
        {
            if (e.Successful)
            {
                SelectedItem = null;
                SearchBarcode = string.Empty;
                Quantity = string.Empty;
                Payment = string.Empty;
                Receipt = new Receipt(ReceiptType.Sale);
                Receipt.ReceiptPosted += OnPosted;
                KeyboardVisualize();
            }
            await _pageService.DisplayAlert("Уведомление", e.Message, "OK");
        }

        private bool CanPost()
        {
            if (_paymentRegex.IsMatch(Payment))
            {
                return CashRegisterContext.CurrentShift != null && CashRegisterContext.CurrentShift.State == ShiftState.Running &&
                        decimal.Parse(Payment.Replace('.', ',')) >= Receipt.TotalPrice && Receipt.Items.Count > 0 && Receipt != null;
            }
            else
            {
                return false;
            }      
        }

        #endregion
    }
}
