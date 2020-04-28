using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.Models.EventsArgs;

namespace Kassa.Models
{
    public class Shift : IShift
    {
        #region Constructors
        public Shift() 
        { 
            
        }
        #endregion
        #region Events
        public event EventHandler<ShiftStartedEventArgs> ShiftStarted;
        public event EventHandler<ShiftFinishedEventArgs> ShiftFinished;
        public event EventHandler<MoneyWithdrawnEventArgs> MoneyWithdrawn;
        public event EventHandler<MoneyAddedEventArgs> MoneyAdded; 
        #endregion
        #region Private props
        private bool _isRunning = false;
        #endregion
        #region Public props
        public long ID { get; private set; }
        public int UserID { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime? EndDateTime { get; private set; }
        public decimal Balance { get; private set; }
        public decimal AddsSum { get; private set; }
        public decimal WithdrawalsSum { get; private set; }
        public decimal SalesSum { get; private set; }
        public decimal ReturnsSum { get; private set; }
        public bool IsFinished { get; private set; }

        #region Navigation props
        public User User { get; private set; }                      
        public IEnumerable<Receipt> Receipts { get; private set; }  
        public IEnumerable<Supply> Supplies { get; private set; }
        public IEnumerable<Disposal> Disposals { get; private set; }
        #endregion
        
        #endregion
        #region Private methods
        
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
                    if (lastShift != null && !lastShift.IsFinished)
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
            if (this._isRunning)
            {
                this.ShiftStarted?.Invoke(this, 
                    new ShiftStartedEventArgs("Невозможно начать уже начатую смену!", false));
                return;
            }

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
                    this.IsFinished = false;
                    ctx.Shifts.Add(this);
                    ctx.SaveChanges();
                    this._isRunning = true;
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
            if (!this._isRunning)
            {
                this.ShiftFinished?.Invoke(this, 
                    new ShiftFinishedEventArgs("Невозможно завершить еще не начатую смену!", false));
                return;
            }
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                string message = string.Empty;
                if (ctx.CheckConnection(out message))
                {
                    this.EndDateTime = DateTime.Now;
                    this.IsFinished = true;
                    ctx.Shifts.Update(this);
                    ctx.SaveChanges();
                    this._isRunning = false;
                    this.ShiftFinished?.Invoke(this, new ShiftFinishedEventArgs(message));
                }
                else
                {
                    this.ShiftFinished?.Invoke(this, new ShiftFinishedEventArgs(message, false));
                }
            }
        }
        public void ChangeSalesStats(decimal money)
        {
            if (this._isRunning)
            {
                this.Balance += money;
                this.SalesSum += money;
            }
        }
        public void ChangeReturnsStats(decimal money)
        {
            if (this._isRunning)
            {
                this.Balance -= money;
                this.ReturnsSum += money;
            }
        }
        public async Task AddMoney(decimal amount)
        {
            if (this._isRunning)
            {
                using (CashRegisterContext ctx = new CashRegisterContext())
                {
                    if (!ctx.CanConnect)
                    {
                        MoneyAdded?.Invoke(this,
                            new MoneyAddedEventArgs("Нет соединения с базой данных!", false));
                        return;
                    }
                    this.AddsSum += amount;
                    this.Balance += amount;
                    ctx.Shifts.Update(this);
                    await ctx.SaveChangesAsync();
                    MoneyAdded?.Invoke(this,
                        new MoneyAddedEventArgs($"Зачислено {AddsSum} руб.!"));
                }
            }
        }
        public async Task WithdrawMoney(decimal amount)
        {
            if (!this._isRunning)
            {
                return;
            }
            
            if (amount > this.Balance)
            {
                MoneyWithdrawn?.Invoke(this,
                    new MoneyWithdrawnEventArgs("Невозможно изъять больше, чем есть! " +
                    $"Не хватает {amount - this.Balance} руб.", false));
                return;
            }
            
            using (CashRegisterContext ctx = new CashRegisterContext())
            {
                if (!ctx.CanConnect)
                {
                    MoneyWithdrawn?.Invoke(this,
                        new MoneyWithdrawnEventArgs("Нет соединения с базой данных!"));
                    return;
                }
                
                this.WithdrawalsSum += amount;
                this.Balance -= amount;
                ctx.Shifts.Update(this);
                await ctx.SaveChangesAsync();
                MoneyWithdrawn?.Invoke(this,
                    new MoneyWithdrawnEventArgs($"Изъято {amount - this.Balance} руб."));
            }
        }
        #endregion
    }
}
