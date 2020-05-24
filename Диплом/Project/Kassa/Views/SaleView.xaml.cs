using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaleView : ContentPage
    {
        public SaleView()
        {
            this.BindingContext = new ViewModels.SaleVM(new OtherComponents.PageService(this));
            InitializeComponent();
        }

        private void KeyboardButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (PaymentLabel.Text.Length == 0 && button.Text == ".")
                {
                    return;
                }
                PaymentLabel.Text += button.Text;
            }
        }

    }
}