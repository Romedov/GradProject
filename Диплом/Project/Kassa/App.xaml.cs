using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Kassa.Views;
using Kassa.Models;
using Kassa.Models.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kassa
{
    public partial class App : Application, INotifyPropertyChanged
    {
        public static ViewModels.ReturnVM ReturnVM { get; set; }
        public App()
        {
            InitializeComponent();
            MainPage = new LoginView();
        }
        public App(string dbPath)
        {
            using (var db = new CashRegisterContext(dbPath))
            {
                db.InitializeDatabase();
            }
            InitializeComponent();
            MainPage = new LoginView();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
            
        }
    }
}
