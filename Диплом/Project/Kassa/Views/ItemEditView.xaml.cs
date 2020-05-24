using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kassa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemEditView : ContentPage
    {
        public ItemEditView()
        {
            InitializeComponent();
            this.BindingContext = new ViewModels.ItemVM(new OtherComponents.PageService(this));
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                entry.Text = entry.Text.Trim().Trim('.');
            }
        }

        private void BarcodeEntry_Unfocused(object sender, FocusEventArgs e)
        {
            var chars = " .,/()*&%^$#@!\"?;:'+=-_[]|\\".ToCharArray();
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