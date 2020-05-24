using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kassa.OtherComponents;

namespace Kassa.Models
{
    public enum ShiftState
    {
        Created,
        Running,
        Finished,
    }
    public class Shift : IShift, INotifyPropertyChanged
    {
        #region Constructors
        public Shift()
        {
            this.State = ShiftState.Created;
            this.Balance = 0;
            this.AddsSum = 0;
            this.WithdrawalsSum = 0;
            this.SalesSum = 0;
            this.ReturnsSum = 0;
        }
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ShiftState> StateChanged;
        #endregion
        #region Private props
        private decimal _balance;
        private decimal _addsSum;
        private decimal _withdrawalsSum;
        private decimal _salesSum;
        private decimal _returnsSum;
        private ShiftState _state;
        #endregion
        #region Public props
        public long ID { get; private set; }
        public int UserID { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime? EndDateTime { get; private set; }
        public decimal Balance 
        {
            get => _balance;
            private set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }
        public decimal AddsSum 
        {
            get => _addsSum;
            private set
            {
                _addsSum = value;
                OnPropertyChanged();
            }
        }
        public decimal WithdrawalsSum
        {
            get => _withdrawalsSum;
            private set
            {
                _withdrawalsSum = value;
                OnPropertyChanged();
            }
        }
        public decimal SalesSum
        {
            get => _salesSum;
            private set
            {
                _salesSum = value;
                OnPropertyChanged();
            }
        }
        public decimal ReturnsSum
        {
            get => _returnsSum;
            private set
            {
                _returnsSum = value;
                OnPropertyChanged();
            }
        }
        public ShiftState State
        {
            get => _state;
            private set
            {
                _state = value;
                OnPropertyChanged();
                ShiftStateNotifier.Notify(value);
            }
        }

        #region Navigation props
        public User User { get; set; }                      
        public IList<Receipt> Receipts { get; set; } = new List<Receipt>();
        public IList<Supply> Supplies { get; set; } = new List<Supply>();
        public IList<Disposal> Disposals { get; set; } = new List<Disposal>();
        #endregion

        #endregion
        #region Protected methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnStateChanged(ShiftState state)
        {
            StateChanged?.Invoke(this, state);
        }
        #endregion
        #region Public methods
        public async Task Start(IUser user)
        {
            if (this.State == ShiftState.Running)
            {
                throw new Exception("Не удалось начать смену: нельзя начать уже начатую смену!");
            }
            else if (this.State == ShiftState.Finished)
            {
                throw new Exception("Не удалось начать смену: нельзя начать уже завершённую смену!");
            }
            else if (user == null)
            {
                throw new Exception("Не удалось начать смену: пользователь не определён!");
            }

            this.User = user as User;
            this.StartDateTime = DateTime.Now;
            this.EndDateTime = null;
            try
            {
                using (CashRegisterContext ctx = new CashRegisterContext())
                {
                    this.State = ShiftState.Running;
                    ctx.Attach(User);
                    await ctx.AddShift(this);

                };
            }
            catch (Exception ex)
            {
                this.State = ShiftState.Created;
                throw new Exception(ex.Message);
            }
        }
        public async Task Finish()
        {
            if (this.State != ShiftState.Running)
            {
                throw new Exception("Не удалось сохранить изменения: смена с данным идентификатором ещё не начата!");
            }

            try
            {
                using (CashRegisterContext ctx = new CashRegisterContext())
                {
                    this.State = ShiftState.Finished;
                    this.EndDateTime = DateTime.Now;
                    await ctx.UpdateShift(this);
                };
            }
            catch (Exception ex)
            {
                this.State = ShiftState.Running;
                throw new Exception(ex.Message);
            }
        }
        public void ChangeSalesStats(decimal money)
        {
            if (this.State == ShiftState.Running)
            {
                this.Balance += money;
                this.SalesSum += money;
            }
        }
        public void ChangeReturnsStats(decimal money)
        {
            if (this.State == ShiftState.Running)
            {
                this.Balance -= money;
                this.ReturnsSum += money;
            }
        }
        public async Task AddMoney(decimal amount)
        {
            if (this.State == ShiftState.Running)
            {
                try
                {
                    using (CashRegisterContext ctx = new CashRegisterContext())
                    {
                        this.AddsSum += amount;
                        this.Balance += amount;
                        await ctx.UpdateShift(this);
                    }
                }
                catch (Exception ex)
                {
                    this.AddsSum -= amount;
                    this.Balance -= amount;
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task WithdrawMoney(decimal amount)
        {
            if (amount > this.Balance)
            {
                throw new Exception($"Невозможно изъять больше средств, чем есть! Не хватает {amount - this.Balance} руб.");
            }

            if (this.State == ShiftState.Running)
            {
                try
                {
                    using (CashRegisterContext ctx = new CashRegisterContext())
                    {
                        this.WithdrawalsSum += amount;
                        this.Balance -= amount;
                        await ctx.UpdateShift(this);
                    }
                }
                catch (Exception ex)
                {
                    this.WithdrawalsSum -= amount;
                    this.Balance += amount;
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion
    }
}
