using Kassa.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiptView : ContentPage
    {
        public ReceiptView(Receipt receipt)
        {
            this.BindingContext = new ViewModels.ReceiptVM(receipt);
            InitializeComponent();
        }
        private async void ReturnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}