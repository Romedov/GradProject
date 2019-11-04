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
        private ItemParent _selectedItem = null;
        private RelayCommand _relayCommand;
        private Item _item;
        private string _freePrice="";
        private string _iId="";
        private Button _keyBtn;
        #endregion
        #region Public properties
        public ObservableCollection<ItemParent> ItemsToSell { get; set; } = new ObservableCollection<ItemParent>();
        public ItemParent SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        public string FreePrice
        {
            get { return _freePrice; }
            set { _freePrice = value; OnPropertyChanged(); }
        }
        public Button KeyBtn
        {
            get { return _keyBtn; }
            set { _keyBtn = value; OnPropertyChanged(); }
        }
        public string IId
        {
            get { return _iId; }
            set { _iId = value; OnPropertyChanged(); }
        }
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
                        Item item = Item.GetItem(this, IId);
                        if (item != null)
                        {
                            ItemParent prevItem = ItemsToSell.FirstOrDefault(i => i.IId == item.IId);
                            if(prevItem == null)
                            {
                                ItemsToSell.Add(item);
                            }
                            else
                            {
                                ItemsToSell.FirstOrDefault(i => i.IId == item.IId).Number++;
                            } 
                        }
                        IId = "";
                    },
                    (obj) => IId.Length > 0));
            }
        }
        public RelayCommand AddFreeCommand
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        ItemsToSell.Add(new FreeItem(App.ShiftVM.CurrentShift.SId, Convert.ToDecimal(FreePrice)));
                        FreePrice = "";
                    },
                    (obj) => FreePrice.Length > 0));
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
                        ItemParent item = obj as ItemParent;
                        if (item != null)
                        {
                            ItemsToSell.Remove(item);
                        }
                    },
                    (obj) => ItemsToSell.Count > 0));
            }
        }
        public RelayCommand KeyboardCommand
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        Button btn = obj as Button;
                        switch (btn.Content)
                        {
                            case "X":
                                FreePrice = "";
                                break;
                            case "BckSpc":
                                if(FreePrice.Length>0)
                                {
                                    FreePrice = FreePrice.Remove(FreePrice.Length - 1);
                                }
                                break;
                            case ",":
                                if (FreePrice.Contains(',')) { break; }
                                FreePrice += btn.Content;
                                break;
                            default:
                                if (FreePrice.IndexOf(',') == 0)
                                {
                                    break;
                                }
                                if (FreePrice.IndexOf(',') != -1 && FreePrice.Substring(FreePrice.IndexOf(',')).Length == 3)
                                {
                                    break;
                                }
                                FreePrice += btn.Content;
                                break;
                        }
                    }));
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
