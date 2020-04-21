using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCoreTest.Models.Interfaces
{
    public interface IItem
    {
        #region Constructors
        #endregion
        #region Events
        #endregion
        #region Props
        int ID { get; }
        string Name { get; }
        float Quantity { get; set; }
        decimal Price { get; }
        byte Discount { get; }
        #endregion
        #region Methods
        #endregion
    }
}
