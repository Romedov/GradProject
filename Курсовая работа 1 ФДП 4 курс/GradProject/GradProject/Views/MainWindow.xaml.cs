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
using GradProject.ViewModels;

namespace GradProject.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.ShiftVM;
        }
        private void PageSwitch(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "SellBtn":
                    MainAction.Source = new Uri("SellView.xaml", UriKind.Relative);
                    Grid.SetRow(PageIndicator, 0);
                    break;
                case "ReturnBtn":
                    MainAction.Source = new Uri("ReturnView.xaml", UriKind.Relative);
                    Grid.SetRow(PageIndicator, 1);
                    break;
                case "ShiftBtn":
                    MainAction.Source = new Uri("ShiftView.xaml", UriKind.Relative);
                    Grid.SetRow(PageIndicator, 2);
                    break;
                case "ExitBtn":
                    Grid.SetRow(PageIndicator, 3);
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}
