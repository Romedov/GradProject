using System;
using System.ComponentModel;
using Kassa.Models;
using Xamarin.Forms;
using Kassa.Views.UserControls;

namespace Kassa.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            this.BindingContext = new ViewModels.MainPageVM(new OtherComponents.PageService(this));
            this.MasterBehavior = MasterBehavior.Popover;
            this.Detail = new NavigationPage(new ShiftView())
            {
                BarBackgroundColor = Color.ForestGreen,
            };
            NavigationPage.SetHasNavigationBar(this, false);
            this.InitializeComponent();
            if (CashRegisterContext.CurrentUser.Role == Roles.Regular)
            {
                UserButton.IsVisible = false;
            }
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            var control = sender as FrameButton;
            switch (control.ClassId)
            {
                case "SaleBtn":
                    this.Detail = new NavigationPage(new SaleView())
                    {
                        BarBackgroundColor = Color.ForestGreen,
                    };
                    this.IsPresented = false;
                    break;
                case "ReturnBtn":
                    this.Detail = new NavigationPage(new ReturnView())
                    {
                        BarBackgroundColor = Color.ForestGreen,
                    };
                    this.IsPresented = false;
                    break;
                case "ShiftBtn":
                    this.Detail = new NavigationPage(new ShiftView())
                    {
                        BarBackgroundColor = Color.ForestGreen,
                    };
                    this.IsPresented = false;
                    break; 
                case "UserBtn":
                    this.Detail = new NavigationPage(new UserMenuPage())
                    {
                        BarBackgroundColor = Color.ForestGreen,
                    };
                    this.IsPresented = false;
                    break;
                case "ItemBtn":
                    this.Detail = new NavigationPage(new ItemMenuPage())
                    {
                        BarBackgroundColor = Color.ForestGreen,
                    };
                    this.IsPresented = false;
                    break;
            }
        }
    }
}