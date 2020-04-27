﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models.Interfaces
{
    interface IEditableItem
    {
        #region Constructors
        #endregion
        #region Events
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
