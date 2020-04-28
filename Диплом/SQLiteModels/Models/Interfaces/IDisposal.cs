using System;
using Kassa.Models.EventsArgs;

namespace Kassa.Models.Interfaces
{
    public interface IDisposal
    {
        public event EventHandler<DisposalRegisteredEventArgs> Registered;
        public long ShiftID { get; }
        public long ItemID { get; }
        string Basis { get; set; }
        float Quantity { get; set; }
        DateTime DisposalDT { get; }

        void Register();
    }
}