using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            var vm = new ViewModels.LoginVM(new OtherComponents.PageService(this));
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void FrameButton_Tapped(object sender, EventArgs e)
        {
            LoginEntry.Unfocus();
            PasswordEntry.Unfocus();
        }
    }
}