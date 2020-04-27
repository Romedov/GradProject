using System;
using Kassa.Models.EventsArgs;

namespace Kassa.Models.Interfaces
{
    public interface ISupply
    {
        event EventHandler<SupplyRegisteredEventArgs> Registered;
        long ShiftID { get; }
        long ItemID { get; }
        string Basis { get; set; }
        float Quantity { get; set; }
        decimal Price { get; set; }
        DateTime SupplyDT { get; }

        void Register();
    }
}