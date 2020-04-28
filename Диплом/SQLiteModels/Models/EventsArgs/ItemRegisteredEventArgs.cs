namespace Kassa.Models.EventsArgs
{
    public class ItemRegisteredEventArgs : EventArgsBase
    {
        public ItemRegisteredEventArgs(string message, bool successful = true) : base(message, successful)
        {
            
        }
    }
}