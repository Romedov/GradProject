using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GradProject
{
    public partial class FreeItem
    {
        #region Constructors
        public FreeItem(long sId)
        {
            this.SId = sId;
            this.CashSum = 0;
        }
        public FreeItem() { }
        #endregion
        #region Public properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SId { get; set; }

        public decimal CashSum { get; set; }
        #endregion
    }
}
