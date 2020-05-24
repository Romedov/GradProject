using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReturnView : ContentPage
    {
        public ReturnView()
        {
            this.BindingContext = new ViewModels.ReturnVM(new OtherComponents.PageService(this));
            InitializeComponent();
        }

    }
}