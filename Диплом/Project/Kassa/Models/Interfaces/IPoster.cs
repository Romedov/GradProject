
namespace Kassa.Models.Interfaces
{
    public interface IPoster
    {
        bool TryPost(IPostable receipt, IShift shift, out string message, decimal money = 0);
    }
}
