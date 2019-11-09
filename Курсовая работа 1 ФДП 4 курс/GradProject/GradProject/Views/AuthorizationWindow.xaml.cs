using System;
using System.Windows;

namespace GradProject.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            this.DataContext = App.ShiftVM;
        }
        private void AuthEventer(object sender, EventArgs e) //передача введенного пароля
        {
            App.ShiftVM.EnteredPassword = Password.Password;
            this.DialogResult = true;
        }
    }
}
