using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GradProject
{
    public partial class Return //����� �������� �������
    {
        #region Constructors
        public Return() { }
        public Return(Shift shift, Item item)
        {
            SId = shift.SId;
            IId = item.IId;
            Number = item.Number;
        }
        #endregion
        #region Public Properties
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SId { get; set; } //id �����

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string IId { get; set; } //id ������

        public long Number { get; set; } //����������
        #endregion
    }
}
