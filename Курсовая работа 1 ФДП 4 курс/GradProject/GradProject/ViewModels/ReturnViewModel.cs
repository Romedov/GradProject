using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GradProject.Models;

namespace GradProject.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для ReturnView.xaml
    /// </summary>
    public class ReturnViewModel : INotifyPropertyChanged //VM возврата
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Constructors
        public ReturnViewModel()
        {
            Item.ItemSearching += OnItemSearch;
        }
        #endregion
        #region Private fields
        private ItemParent _selectedItem = null;
        private RelayCommand _relayCommand;
        private string _freePrice = "";
        private string _iId = "";
        private Button _keyBtn;
        private decimal _moneyToReturn = 0;
        decimal _conclusion = 0;
        #endregion
        #region Public properties
        public ObservableCollection<ItemParent> ItemsToReturn { get; set; } = new ObservableCollection<ItemParent>(); //список товаров
        public ItemParent SelectedItem //выбранный из списка товар
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        public decimal Conclusion //итоговая цена
        {
            get { return _conclusion; }
            set { _conclusion = value; OnPropertyChanged(); }
        }
        public decimal MoneyToReturn //средства к возврату
        {
            get { return _moneyToReturn; }
            set { _moneyToReturn = value; OnPropertyChanged(); }
        }
        public string FreePrice //цена товара, нерегистрируемого в БД
        {
            get { return _freePrice; }
            set { _freePrice = value; OnPropertyChanged(); }
        }
        public Button KeyBtn //нажатая на клавиатуре ввода цены товара, нерегистрируемого в БД, кнопка
        {
            get { return _keyBtn; }
            set { _keyBtn = value; OnPropertyChanged(); }
        }
        public string IId //id товара
        {
            get { return _iId; }
            set { _iId = value; OnPropertyChanged(); }
        }
        #endregion
        #region Commands
        public RelayCommand AddCommand //добавление товара, регистрируемого в БД
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
                            ItemParent prevItem = ItemsToReturn.FirstOrDefault(i => i.IId == item.IId);
                            if (prevItem == null)
                            {
                                ItemsToReturn.Add(item);
                            }
                            else
                            {
                                ItemsToReturn.FirstOrDefault(i => i.IId == item.IId).Number++;
                            }
                            Conclusion = ItemsToReturn.Conclude();
                        }
                        IId = "";
                    },
                    (obj) => IId.Length > 0));
            }
        }
        public RelayCommand AddFreeCommand //добавление товара, не регистрируемого в БД
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        ItemsToReturn.Add(new FreeItem(App.ShiftVM.CurrentShift.SId, Convert.ToDecimal(FreePrice)));
                        Conclusion = ItemsToReturn.Conclude();
                        FreePrice = "";
                    },
                    (obj) => FreePrice.Length > 0));
            }
        }
        public RelayCommand RemoveCommand //удаление товара из списка
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
                            ItemsToReturn.Remove(item);
                            if (ItemsToReturn.Count() > 0)
                            {
                                ItemsToReturn.Conclude();
                            }
                            else
                            {
                                Conclusion = 0;
                            }
                        }
                    },
                    (obj) => ItemsToReturn.Count > 0));
            }
        }
        public RelayCommand ReturnCommand //команда осуществления возврата товара
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        Shift currShift = App.ShiftVM.CurrentShift;
                        if (Conclusion <= currShift.CurrentCash)
                        {
                            foreach(var item in ItemsToReturn)
                            {
                                item.ReturnItemAsync(currShift);
                            }
                            ItemsToReturn.Clear();
                            MessageBox.Show($"Передайте клиенту {Conclusion} руб.");
                            Conclusion = (decimal)0.00;
                        }
                        else
                        {
                            MessageBox.Show($"В кассе не достаточно денег: {Conclusion - currShift.CurrentCash} руб.");
                        }
                    },
                    (obj) => ItemsToReturn.Count > 0));
            }
        }
        public RelayCommand KeyboardCommand //комада ввода товара, нерегистрируемого в БД
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
                                if (FreePrice.Length > 0)
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
        private void OnItemSearch(object sender, ItemSearchEventArgs e) //обработцик события поиска товара
        {
            if (e.IsSuccessful == false)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
