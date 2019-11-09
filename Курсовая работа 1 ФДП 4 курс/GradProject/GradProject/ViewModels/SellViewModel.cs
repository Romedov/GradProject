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
using GradProject.Views;

namespace GradProject.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для SellView.xaml
    /// </summary>
    public class SellViewModel : INotifyPropertyChanged //VM продажи
    {
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
        private string _freePrice="";
        private string _iId="";
        private Button _keyBtn;
        private Window _wnd = null;
        private decimal _moneyToPay=0;
        decimal _itemsPriceSum = 0;
        decimal _conclusion = 0;
        #endregion
        #region Public properties
        public ObservableCollection<ItemParent> ItemsToSell { get; set; } = new ObservableCollection<ItemParent>(); //список продаваемых товаров
        public ItemParent SelectedItem //выбранный товар
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        public decimal Conclusion //итоговая цена
        {
            get { return _conclusion; }
            set { _conclusion = value; OnPropertyChanged(); }
        }
        public decimal MoneyToPay //средства к оплате
        {
            get { return _moneyToPay; }
            set { _moneyToPay = value; OnPropertyChanged(); }
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
        public RelayCommand AddCommand //добавление товара в список
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
                            Conclusion = ItemsToSell.Conclude();
                        }
                        IId = "";
                    },
                    (obj) => IId.Length > 0));
            }
        } 
        public RelayCommand AddFreeCommand //добавление "свободного" товара в список
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        ItemsToSell.Add(new FreeItem(App.ShiftVM.CurrentShift.SId, Convert.ToDecimal(FreePrice)));
                        Conclusion = ItemsToSell.Conclude();
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
                            ItemsToSell.Remove(item);
                            if (ItemsToSell.Count() > 0)
                            {
                                ItemsToSell.Conclude();
                            }
                            else
                            {
                                Conclusion = 0;
                            }
                        }
                    },
                    (obj) => ItemsToSell.Count > 0));
            }
        }
        public RelayCommand SellCommand //продажа
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        bool isEnough = true;
                        foreach (var item in ItemsToSell)
                        {
                            if (item.IId != "Free")
                            {
                                isEnough = ((Item)item).NumberCheck();
                                if (!isEnough) { break; }
                            }
                        }
                        if (isEnough)
                        {
                            _wnd = new PayBackWindow();
                            _itemsPriceSum = ItemsToSell.Sum(i => i.Price);
                            _wnd.ShowDialog();
                        }
                    },
                    (obj) => ItemsToSell.Count > 0));
            }
        }
        public RelayCommand PayCashCommand //оплата
        {
            get
            {
                _relayCommand = null;
                return _relayCommand ??
                    (_relayCommand = new RelayCommand(obj =>
                    {
                        var currShift = App.ShiftVM.CurrentShift;
                        if (MoneyToPay > currShift.CurrentCash)
                        {
                            MessageBox.Show($"В кассе недостаточно денег для сдачи: {MoneyToPay - currShift.CurrentCash} руб.");
                        }
                        else
                        {
                            foreach(var item in ItemsToSell)
                            {
                                item.SellItemAsync(currShift);
                            }
                            _wnd.Close();
                            _wnd = null;
                            ItemsToSell.Clear();
                            if ((Conclusion - MoneyToPay) != 0)
                            {
                                MessageBox.Show(string.Format("Выдайте сдачу: {0} руб.", MoneyToPay - Conclusion));
                            }
                            Conclusion = 0;
                        }
                    },
                    (obj) => MoneyToPay >= _itemsPriceSum));
            }
        }
        public RelayCommand KeyboardCommand //команда ввода "свободного" товара
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
        private void OnItemSearch(object sender, ItemSearchEventArgs e) //обработчик события поиска товара в БД
        {
            if (e.IsSuccessful == false)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
