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
    /// Логика взаимодействия для WithdrawShiftWindow.xaml
    /// </summary>
    public partial class WithdrawShiftWindow : Window
    {
        public WithdrawShiftWindow()
        {
            InitializeComponent();
            this.DataContext = App.ShiftVM;
        }
        private void MoneyTypeInPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tbne = sender as TextBox;
            if ((!Char.IsDigit(e.Text, 0)) && (e.Text != "."))
            {
                e.Handled = true;
            }
            else
            {
                if ((e.Text == ".") && ((tbne.Text.IndexOf(".") != -1) || (tbne.Text == "")))
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
