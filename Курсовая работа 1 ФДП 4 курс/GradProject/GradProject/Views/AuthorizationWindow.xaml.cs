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
using GradProject.Models;
using GradProject.ViewModels;

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
        private void AuthEventer(object sender, EventArgs e)
        {
            App.ShiftVM.EnteredPassword = Password.Password;
            this.DialogResult = true;
        }
    }
}
