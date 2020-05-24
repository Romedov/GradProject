using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kassa.OtherComponents
{
    public class PageService : IPageService
    {
        public PageService(Page page)
        {
            this.Page = page;
        }

        public Page Page { get; }

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await Page.DisplayAlert(title, message, ok, cancel);
        }

        public async Task DisplayAlert(string title, string message, string ok)
        {
            await Page.DisplayAlert(title, message, ok);
        }

        public async Task<Page> PopAsync()
        {
            return await Page.Navigation.PopAsync();
        }

        public async Task PushAsync(Page page)
        {
            await Page.Navigation.PushAsync(page);
        }

        public async Task PushModalAsync(Page page)
        {
            await Page.Navigation.PushModalAsync(page);
        }
    }
}
