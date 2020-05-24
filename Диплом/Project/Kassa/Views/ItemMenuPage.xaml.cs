using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemMenuPage : ContentPage
    {
        public ItemMenuPage()
        {
            InitializeComponent();
            if (Models.CashRegisterContext.CurrentUser.Role == Models.Roles.Regular)
            {
                RegisterButton.IsVisible = false;
                EditButton.IsVisible = false;
            }
        }

        private async void ItemRegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemRegistrationView());
        }
        private async void ItemEditButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemEditView());
        }
        private async void ItemSupplyButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SupplyView());
        }
        private async void ItemDisposalButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DisposalView());
        }
    }
}