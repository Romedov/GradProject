using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class Shift : IShift
    {
        #region Constructors
        public Shift() { }
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
        public User User { get; private set; }                      //nav prop
        public IEnumerable<Receipt> Receipts { get; private set; }  //nav
        public IEnumerable<Supply> Supplies { get; private set; }   //nav
        #endregion
        #region Private methods
        //private bool LastUnfinishedRestored(CashRegisterContext ctx)
        //{
        //    var lastShift = ctx.Shifts.Reverse().FirstOrDefault();
        //    if (lastShift.EndDateTime == null && lastShift != null)
        //    {
        //        this.ID = lastShift.ID;
        //        this.UserID = lastShift.UserID;
        //        this.StartDateTime = lastShift.StartDateTime;
        //        this.EndDateTime = null;
        //        this.Balance = lastShift.Balance;
        //        this.AddsSum = lastShift.AddsSum;
        //        this.WithdrawalsSum = lastShift.WithdrawalsSum;
        //        this.SalesSum = lastShift.SalesSum;
        //        this.ReturnsSum = lastShift.ReturnsSum;
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        #endregion
        #region Public methods
        public bool TryRestoreLastUnfinished(out IShift shift)
        {
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                string message = string.Empty;
                if (ctx.CheckConnection(out message))
                {
                    var lastShift = ctx.Shifts.Reverse().FirstOrDefault();
                    if (lastShift != null && lastShift.EndDateTime == null)
                    {
                        shift = lastShift;
                        return true;
                    }
                    else
                    {
                        shift = new Shift();
                        return false;
                    }
                }
                else
                {
                    shift = new Shift();
                    return false;
                }            
            }
        }
        public void Start(IUser user)
        {
            this.UserID = user.ID;
            this.User = user as User;
            this.StartDateTime = DateTime.Now;
            this.EndDateTime = null;
            this.Balance = 0;
            this.AddsSum = 0;
            this.WithdrawalsSum = 0;
            this.SalesSum = 0;
            this.ReturnsSum = 0;
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                string message = string.Empty;
                if (ctx.CheckConnection(out message))
                {
                    
                }
                else
                {

                }
            }
        }
        public void Finish()
        {

        }
        public void Sell(Receipt receipt)
        {

        }
        public void Return(Receipt receipt)
        {

        }
        public async Task AddMoney(decimal amount)
        {

        }
        public async Task WithdrawMoney(decimal amount)
        {

        }
        #endregion
    }
}
