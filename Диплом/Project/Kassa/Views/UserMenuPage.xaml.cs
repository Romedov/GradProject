using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserMenuPage : ContentPage
    {
        public UserMenuPage()
        {
            InitializeComponent();
        }

        private async void UserCreateButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserRegistrationView());
        }
        private async void UserEditButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserEditView());
        }
    }
}