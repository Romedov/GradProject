namespace Kassa.Models.EventsArgs
{
    public class MoneyAddedEventArgs : EventArgsBase
    {
        public MoneyAddedEventArgs(string message, bool successful = true) : base(message, successful)
        {
            
        }
    }
}