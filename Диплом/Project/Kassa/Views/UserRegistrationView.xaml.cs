using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserRegistrationView : ContentPage
    {
        public UserRegistrationView()
        {
            InitializeComponent();
            this.BindingContext = new ViewModels.UserRegisterVM(new OtherComponents.PageService(this));
        }
    }
}