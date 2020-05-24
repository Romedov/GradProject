using Kassa.Models;
using Kassa.ViewModels;

namespace Kassa.OtherComponents
{
    public static class ShiftStateNotifier
    {
        public static MainPageVM MainPageVM { get; set; }
        public static void Notify(ShiftState state)
        {
            MainPageVM?.OnStateChanged(state);
        }
    }
}
