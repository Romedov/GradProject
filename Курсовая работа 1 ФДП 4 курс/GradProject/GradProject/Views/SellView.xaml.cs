using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GradProject.Views
{
    /// <summary>
    /// Логика взаимодействия для SellView.xaml
    /// </summary>
    public partial class SellView : Page
    {
        public SellView()
        {
            InitializeComponent();
            this.DataContext = App.SellVM;
        }
        private void ValidatePTI(object sender, TextCompositionEventArgs e) //только цифры
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }
        private void ValidatePKD(object sender, KeyEventArgs e) //только цифры
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
