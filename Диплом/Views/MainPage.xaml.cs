using System;
using System.Collections.Generic;
using System.ComponentModel;
using Kassa.Models;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.MasterBehavior = MasterBehavior.Popover;
            this.Detail = new NavigationPage(new SalePage());
            //using (CashRegisterContext ctx = new CashRegisterContext())
            //{
            //    ctx.Database.EnsureDeleted();
            //    ctx.Database.EnsureCreated();
            //}
        }
    }
}