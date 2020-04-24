using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Kassa.OtherComponents.Interfaces;

namespace Kassa.OtherComponents
{
    public class PageService : IPageService
    {
        public async Task<bool> DisplayAlert(Page page, string title, string message, string ok, string cancel)
        {
            return await page.DisplayAlert(title, message, ok, cancel);
        }

        public async Task DisplayAlert(Page page, string title, string message, string ok)
        {
            await page.DisplayAlert(title, message, ok);
        }

        public async Task<Page> PopAsync(Page page)
        {
            return await page.Navigation.PopAsync();
        }

        public async Task PushAsync(Page page)
        {
            await PushAsync(page);
        }

        public async Task PushModalAsync(Page page)
        {
            await PushModalAsync(page);
        }
    }
}
