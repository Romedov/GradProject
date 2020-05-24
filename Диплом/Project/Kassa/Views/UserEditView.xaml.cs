using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEditView : ContentPage
    {
        public UserEditView()
        {
            this.BindingContext = new ViewModels.UserEditVM(new OtherComponents.PageService(this));
            InitializeComponent();
        }
    }
}