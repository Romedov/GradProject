namespace Kassa.Models.EventsArgs
{
    public class DisposalRegisteredEventArgs : EventArgsBase
    {
        public DisposalRegisteredEventArgs(string message, bool successful = true) : base(message, successful)
        {
            
        }
    }
}