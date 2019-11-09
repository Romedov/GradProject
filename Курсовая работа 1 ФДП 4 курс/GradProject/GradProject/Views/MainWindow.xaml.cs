using System;
using System.Windows;
using System.Windows.Controls;

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
            this.Closing += App.ShiftVM.OnWindowClosing;
        }
        private void PageSwitch(object sender, EventArgs e) //переход по страницам
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
