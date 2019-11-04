using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Runtime.CompilerServices;

namespace GradProject
{
    public partial class FreeItem : ItemParent, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Constructors
        public FreeItem(long sId)
        {
            this.SId = sId;
            this.CashSum = 0;
        }
        public FreeItem(long sId, decimal price)
        {
            this.SId = sId;
            this.CashSum = 0;
            this.IId = "Free";
            this.Name = "Товар";
            this.Price = price;
            this.Number = 1;
        }
        public FreeItem() { }
        #region Private fields
        private long _number;
        #endregion
        #endregion
        #region Public properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SId { get; set; }

        public decimal CashSum { get; set; }
        [NotMapped]
        public override string IId { get; protected set; }
        [NotMapped]
        public override string Name { get; protected set; }
        [NotMapped]
        public override decimal Price { get; protected set; }
        [NotMapped]
        public override long Number
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public override void SellItem(Shift currShift)
        {

        }
        #endregion
    }
}
