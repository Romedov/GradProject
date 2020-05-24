using System;
using System.Threading.Tasks;
using Kassa.Models.EventsArgs;

namespace Kassa.Models.Interfaces
{
    public interface IDisposal
    {
        long ShiftID { get; }
        long ItemID { get; }
        string Basis { get; set; }
        float Quantity { get; set; }
        DateTime DisposalDT { get; }

        Shift Shift { get; set; }
        Item Item { get; set; }
        Task Register(ISellableItem item);
    }
}