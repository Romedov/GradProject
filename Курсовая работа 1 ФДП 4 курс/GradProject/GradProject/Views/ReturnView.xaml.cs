using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace GradProject.Views
{
    /// <summary>
    /// Логика взаимодействия для ReturnView.xaml
    /// </summary>
    public partial class ReturnView : Page
    {
        public ReturnView()
        {
            InitializeComponent();
            this.DataContext = App.ReturnVM;
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
        private void ConcludeKU(object sender, KeyEventArgs e) //обновление итога
        {
            App.ReturnVM.Conclusion = App.ReturnVM.ItemsToReturn.Conclude();
        }
        private void OnClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            App.ReturnVM.KeyBtn = btn;
        }
    }
}
