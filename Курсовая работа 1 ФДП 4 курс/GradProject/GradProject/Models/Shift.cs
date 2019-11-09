using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using GradProject.Models;

namespace GradProject
{
    public partial class Shift : INotifyPropertyChanged //класс смены
    {
        #region Constructors
        public Shift()
        {
            CurrentCash = 0;
            CashReceived = 0;
            CashAdded = 0;
            CashWithdrawn = 0;
            CashReturned = 0;
            IsActive = false;
        }
        public Shift(IUser<User> user, decimal prevShiftCashLeft)
        {
            UId = user.GetInstance().UId;
            StartDateTime = DateTime.Now;
            CurrentCash = prevShiftCashLeft;
            CashReceived = 0;
            CashAdded = 0;
            CashWithdrawn = 0;
            CashReturned = 0;
            IsActive = true;
        }
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public static event EventHandler<ShiftTransactionEventArgs> TransactionCompleted; //событие внесени€/изъ€ти€ средств
        #endregion
        #region Private fields
        [NotMapped]
        private string _uId;
        [NotMapped]
        private decimal _cashReceived;  //сумма продаж
        [NotMapped]
        private decimal _cashReturned;  //сумма возвратов
        [NotMapped]
        private decimal _cashAdded;     //сумма внесений
        [NotMapped]
        private decimal _cashWithdrawn; //сумма изъ€тий
        [NotMapped]
        private decimal _currentCash;   //текуща€ сумма в кассе
        #endregion
        #region Public properties
        [NotMapped]
        public bool IsActive { get; set; } //флаг активности
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SId { get; private set; } //id смены

        [Required]
        [StringLength(50)]
        public string UId //id оператора
        {
            get { return _uId; }
            set { _uId = value; OnPropertyChanged(); }
        }

        public DateTime StartDateTime { get; private set; } //дата и врем€ начала смены

        public DateTime? EndDateTime { get; private set; } //дата и врем€ конца смены

        public decimal CurrentCash //текущий баланс
        {
            get { return _currentCash; }
            set { _currentCash = value; OnPropertyChanged(); }
        }

        public decimal CashReceived //сумма продаж
        {
            get { return _cashReceived; }
            set { _cashReceived = value; OnPropertyChanged(); }
        }

        public decimal CashReturned //сумма возвратов
        {
            get { return _cashReturned; }
            set { _cashReturned = value; OnPropertyChanged(); }
        }

        public decimal CashAdded //сумма внесений
        {
            get { return _cashAdded; }
            set { _cashAdded = value; OnPropertyChanged(); }
        }

        public decimal CashWithdrawn //сумма изъ€тий
        {
            get { return _cashWithdrawn; }
            set { _cashWithdrawn = value; OnPropertyChanged(); }
        }

        #endregion
        #region Methods
        public static async void AddMoneyAsync(decimal money, Shift shift) //внесение средств
        {
            using (CashboxDataContext db = new CashboxDataContext())
            {
                try
                {
                    db.DBConnectionCheck();
                    db.Shifts.Attach(shift);
                    shift.CashAdded += money;
                    shift.CurrentCash += money;
                    await db.SaveChangesAsync();
                    TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs("—редства успешно добавлены!", true));
                }
                catch (Exception e)
                {
                    TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs(e.Message + "\n—редства добавлены не были!", false));
                }
            }
        }
        public static async void WithdrawMoneyAsync(decimal money, Shift shift) //изъ€тие средств
        {
            using (CashboxDataContext db = new CashboxDataContext())
            {
                try
                {
                    db.DBConnectionCheck();
                    db.Shifts.Attach(shift);
                    if (shift.CurrentCash >= money)
                    {
                        shift.CurrentCash -= money;
                        shift.CashWithdrawn += money;
                        await db.SaveChangesAsync();
                        TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs("—редства успешно изъ€ты!", true));
                    }
                    else
                    {
                        TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs("Ќевозможно изъ€ть введенную сумму, так как в кассе находитс€ меньше средств, чем требуетс€", false));
                    }
                }
                catch (Exception e)
                {
                    TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs(e.Message + "\n—редства изъ€ты не были!", false));
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public static Shift ShiftStart(IUser<User> user) //старт смены
        {
            using (CashboxDataContext db = new CashboxDataContext())
            {
                try
                {
                    db.DBConnectionCheck();
                    Shift prevShift = db.Shifts.OrderByDescending(sh => sh.SId).FirstOrDefault();
                    decimal currCash = 0;
                    currCash = prevShift != null ? prevShift.CurrentCash : 0;
                    Shift currShift = new Shift(user, currCash);
                    db.Shifts.Add(currShift);
                    db.SaveChanges();
                    return currShift;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
            }
        }
        public bool EndShift() //завершение смены
        {
            using (CashboxDataContext db = new CashboxDataContext())
            {
                try
                {
                    db.DBConnectionCheck();
                    db.Shifts.Attach(this);
                    EndDateTime = DateTime.Now;
                    db.SaveChanges();
                    CurrentCash = 0;
                    CashReceived = 0;
                    CashAdded = 0;
                    CashWithdrawn = 0;
                    CashReturned = 0;
                    IsActive = false;
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }
        }
        #endregion
    }
}
