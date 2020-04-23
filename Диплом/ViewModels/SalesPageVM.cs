using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Kassa.Models;
using Kassa.Models.Interfaces;

namespace Kassa.ViewModels
{
    public class SalesPageVM : ViewModelBase
    {
        #region Constructors
        public SalesPageVM()
        {

        }
        #endregion
        #region Events
        #endregion
        #region Private props
        private Receipt _receipt;
        #endregion
        #region Public props
        public Receipt Receipt 
        {
            get => _receipt;
            set
            {
                _receipt = value;
                OnPropertyChanged();
            }
        }
        public ReadOnlyObservableCollection<ReceiptItem> Items
        {
            get => new ReadOnlyObservableCollection<ReceiptItem>(Receipt.Items);
        }
        
        #endregion
        #region Private methods
        #endregion
        #region Public methods

        #endregion
        #region Operators

        #endregion
    }
}
