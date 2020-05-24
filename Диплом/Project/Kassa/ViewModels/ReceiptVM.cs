using Kassa.Models;
using System.Linq;
using Xamarin.Forms;

namespace Kassa.ViewModels
{
    public class ReceiptVM : ViewModelBase
    {
        #region Public

        public ReceiptVM(Receipt receipt)
        {
            RegisteredReceipt = receipt;
            SelectedItem = null;
            AddCommand = new Command(() => AddToReceipt(), () => SelectedItem != null && !ContainsItem());
        }

        public Receipt RegisteredReceipt 
        {
            get => _registeredReceipt;
            set
            {
                _registeredReceipt = value;
                OnPropertyChanged();
            }
        }

        public ReceiptItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                AddCommand?.ChangeCanExecute();
            }
        }

        public Command AddCommand { get; }

        #endregion

        #region Private

        private Receipt _registeredReceipt;
        private ReceiptItem _selectedItem;

        private void AddToReceipt()
        {
            App.ReturnVM.Receipt.Items.Add(new ReceiptItem(SelectedItem));
            App.ReturnVM.Receipt.ComputeTotalPrice();
            AddCommand?.ChangeCanExecute();
            App.ReturnVM.PostCommand?.ChangeCanExecute();
            App.ReturnVM.RemoveCommand?.ChangeCanExecute();
        }

        public bool ContainsItem()
        {
            return App.ReturnVM.Receipt.Items.SingleOrDefault(i => i.ItemID == SelectedItem.ItemID) != null ? true : false;
        }

        #endregion
    }
}
