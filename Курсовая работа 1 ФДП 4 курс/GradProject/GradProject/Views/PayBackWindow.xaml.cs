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
using System.Windows.Shapes;

namespace GradProject.Views
{
    /// <summary>
    /// Логика взаимодействия для AddShiftWindow.xaml
    /// </summary>
    public partial class PayBackWindow : Window
    {
        public PayBackWindow()
        {
            InitializeComponent();
            this.DataContext = App.SellVM;
        }
        private void MoneyTypeInPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if ((!Char.IsDigit(e.Text, 0)) && (e.Text != "."))
            {
                e.Handled = true;
            }
            if (e.Text == "." && tb.Text.Length == 0)
            {
                e.Handled = true;
            }
            if (tb.Text.Count(ch => ch == '.') == 1 && e.Text == ".")
            {
                e.Handled = true;
            }
            if (tb.Text.IndexOf('.') != -1)
            {
                if (tb.Text.Substring(tb.Text.IndexOf('.')).Length == 3 || (tb.Text.Count(ch => ch == '.') == 1 && e.Text == "."))
                {
                    e.Handled = true;
                }
            }
        }
        private void CloseDialog(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
