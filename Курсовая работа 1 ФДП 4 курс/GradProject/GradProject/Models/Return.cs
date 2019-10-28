using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GradProject
{
    public partial class Return
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string IId { get; set; }

        public long Number { get; set; }
    }
}
