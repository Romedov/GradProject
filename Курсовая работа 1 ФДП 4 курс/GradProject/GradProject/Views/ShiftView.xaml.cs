using System;
using System.Windows.Controls;
using GradProject.ViewModels;

namespace GradProject.Views
{
    /// <summary>
    /// Логика взаимодействия для ShiftView.xaml
    /// </summary>
    public partial class ShiftView : Page
    {
        public ShiftView()
        {
            InitializeComponent();
            this.DataContext = App.ShiftVM;
        }
    }
}
