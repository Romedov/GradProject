using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kassa.OtherComponents.Interfaces
{
    public interface IPageService
    {
        Task PushAsync(Page page);
        Task PushModalAsync(Page page);
        Task<Page> PopAsync(Page page);
        Task<bool> DisplayAlert(Page page, string title, string message, string ok, string cancel);
        Task DisplayAlert(Page page, string title, string message, string ok);
    }
}
