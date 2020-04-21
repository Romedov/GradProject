using EntityFrameworkCoreTest.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCoreTest.Models
{
    public class Shift
    {
        #region Constructors
        public Shift() { }
        public Shift(IUser user)
        {
            UserID = user.ID;
            StartDateTime = DateTime.Now;
            EndDateTime = null;
            Balance = 0;
            AddsSum = 0;
            WithdrawalsSum = 0;
            SalesSum = 0;
            ReturnsSum = 0;
        }
        #endregion
        #region Events
        #endregion
        #region Private props
        #endregion
        #region Public props
        public decimal ID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public decimal Balance { get; set; }
        public decimal AddsSum { get; set; }
        public decimal WithdrawalsSum { get; set; }
        public decimal SalesSum { get; set; }
        public decimal ReturnsSum { get; set; }
        public User User { get; set; }                      //nav prop
        public IEnumerable<Receipt> Receipts { get; set; }  //nav
        public IEnumerable<Supply> Supplies { get; set; }   //nav
        #endregion
        #region Private methods
        #endregion
        #region Public methods
        #endregion
    }
}
