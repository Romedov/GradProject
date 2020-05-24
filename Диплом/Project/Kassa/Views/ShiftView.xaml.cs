using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShiftView : ContentPage
    {
        public ShiftView()
        {
            InitializeComponent();
            this.BindingContext = new ViewModels.ShiftVM(new OtherComponents.PageService(this));
        }

        private void KeyboardButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (SideMoneyLabel.Text.Length == 0 && button.Text == ".")
                {
                    return;
                }
                SideMoneyLabel.Text += button.Text;
            }
        }
    }
}