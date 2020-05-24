using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplyView : ContentPage
    {
        public SupplyView()
        {
            InitializeComponent();
            this.BindingContext = new ViewModels.SupplyVM(new OtherComponents.PageService(this));
        }
        private void BasisEditor_Unfocused(object sender, FocusEventArgs e)
        {
            if (sender is Editor editor)
            {
                editor.Text = editor.Text.Trim().TrimStart(",./()*&%^$#@!\"?;:'+=-_[]|\\".ToCharArray());
            }
        }

        private void NumberEntry_Unfocused(object sender, FocusEventArgs e)
        {
            var chars = " ,/()*&%^$#@!\"?;:'+=-_[]|\\".ToCharArray();
            if (sender is Entry entry)
            {
                foreach (var ch in chars)
                {
                    entry.Text = entry.Text.Replace(ch.ToString(), "");
                }
            }
        }
    }
}