using System;
using Kassa.Models.EventsArgs;

namespace Kassa.Models.Interfaces
{
    interface IEditableItem
    {
        #region Constructors
        #endregion
        #region Events
        event EventHandler<ItemChangesAppliedEventArgs> ChangesApplied;
        event EventHandler<ItemRegisteredEventArgs> Registered;
        #endregion
        #region Props
        long ID { get; }
        string Barcode { get; set; }
        string Name { get; set; }
        float Quantity { get; }
        decimal Price { get; set; }
        byte Discount { get; set; }
        #endregion
        #region Methods
        void ApplyChanges();
        void Register();

        #endregion
    }
}
