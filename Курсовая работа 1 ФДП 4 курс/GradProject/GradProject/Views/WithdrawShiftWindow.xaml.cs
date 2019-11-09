using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
