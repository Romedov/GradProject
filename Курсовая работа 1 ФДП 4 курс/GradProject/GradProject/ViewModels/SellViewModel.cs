using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GradProject.Models;

namespace GradProject.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для SellView.xaml
    /// </summary>
    public class SellViewModel : INotifyPropertyChanged
    {
        //добавить команду продажи
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Constructors

        public SellViewModel()
        {
            Item.ItemSearching += OnItemSearch;
        }
        #endregion
        #region Private fields
        private Item _selectedItem = null;
        private RelayCommand _relayCommand;
        private Item _item;
        #endregion
        #region Public properties
        public ObservableCollection<Item> ItemsToSell { get; set; } = new ObservableCollection<Item>();
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        public string IId { get; set; }
        #endregion
        #region Commands
        public RelayCommand AddCommand
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        //MessageBox.Show(IId);
                        Item item = Item.GetItem(this, IId);
                        if (item != null)
                        {
                            ItemsToSell.Add(item);
                        }
                    }));
            }
        }
        public RelayCommand RemoveCommand
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        Item item = obj as Item;
                        if (item != null)
                        {
                            ItemsToSell.Remove(item);
                        }
                    },
                    (obj) => ItemsToSell.Count > 0));
            }
        }
        #endregion
        #region Methods
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private void OnItemSearch(object sender, ItemSearchEventArgs e)
        {
            if (e.IsSuccessful == false)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
