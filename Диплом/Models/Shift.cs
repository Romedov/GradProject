using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.Models.EventsArgs;

namespace Kassa.Models
{
    public class Shift : IShift, IShiftEventSender
    {
        #region Constructors
        public Shift() { }
        #endregion
        #region Events
        public event Action<IShiftEventSender, ShiftStartedEventArgs> ShiftStarted;
        public event Action<IShiftEventSender, ShiftFinishedEventArgs> ShiftFinished;
        #endregion
        #region Private props
        #endregion
        #region Public props
        public decimal ID { get; private set; }
        public int UserID { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime? EndDateTime { get; private set; }
        public decimal Balance { get; private set; }
        public decimal AddsSum { get; private set; }
        public decimal WithdrawalsSum { get; private set; }
        public decimal SalesSum { get; private set; }
        public decimal ReturnsSum { get; private set; }
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
                    ctx.Shifts.Add(this);
                    ctx.SaveChanges();
                    this.ShiftStarted?.Invoke(this, new ShiftStartedEventArgs(message));
                }
                else
                {
                    this.ShiftStarted?.Invoke(this, new ShiftStartedEventArgs(message, false));
                }
            }
        }
        public void Finish()
        { 
            //TODO: если будет время предусмотреть защиту от уже завершившейся смены с таким же id
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                string message = string.Empty;
                if (ctx.CheckConnection(out message))
                {
                    this.EndDateTime = DateTime.Now;
                    ctx.Shifts.Update(this);
                    ctx.SaveChanges();
                    this.ShiftFinished?.Invoke(this, new ShiftFinishedEventArgs(message));
                }
                else
                {
                    this.ShiftFinished?.Invoke(this, new ShiftFinishedEventArgs(message, false));
                }
            }
        }
        public void Sell(IPostable receipt, decimal money)
        {
            if (receipt != null && money > 0 && receipt.ReceiptType == ReceiptType.Sale)
            {
                decimal result = receipt.Post(new PostSaleReceipt(receipt), this, money);
                if (result > Convert.ToDecimal(0))
                {
                    this.Balance += result;
                    this.SalesSum += result;
                    //TODO: create and invoke a proper event
                }
                else
                {
                    //TODO: create and invoke a proper event
                }
            }
            else
            {
                //TODO: create and invoke a proper event
            }
        }
        public void Return(IPostable receipt)
        {
            if (receipt != null && receipt.ReceiptType == ReceiptType.Return)
            {
                decimal result = receipt.Post(new PostReturnReceipt(receipt), this);
                if (result > Convert.ToDecimal(0))
                {
                    this.Balance -= result;
                    this.ReturnsSum += result;
                    //TODO: create and invoke a proper event
                }
                else
                {
                    //TODO: create and invoke a proper event
                }
            }
            else
            {
                //TODO: create and invoke a proper event
            }
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
