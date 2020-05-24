using System;
using System.Threading.Tasks;

namespace Kassa.Models.Interfaces
{
    public interface ISupply
    {
        long ShiftID { get; }
        long ItemID { get; }
        string Basis { get; set; }
        float Quantity { get; set; }
        decimal Price { get; set; }
        DateTime SupplyDT { get; }

        Task Register(IEditableItem item);
    }
}