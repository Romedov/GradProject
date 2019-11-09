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
    public partial class Shift : INotifyPropertyChanged //����� �����
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
        public static event EventHandler<ShiftTransactionEventArgs> TransactionCompleted; //������� ��������/������� �������
        #endregion
        #region Private fields
        [NotMapped]
        private string _uId;
        [NotMapped]
        private decimal _cashReceived;  //����� ������
        [NotMapped]
        private decimal _cashReturned;  //����� ���������
        [NotMapped]
        private decimal _cashAdded;     //����� ��������
        [NotMapped]
        private decimal _cashWithdrawn; //����� �������
        [NotMapped]
        private decimal _currentCash;   //������� ����� � �����
        #endregion
        #region Public properties
        [NotMapped]
        public bool IsActive { get; set; } //���� ����������
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SId { get; private set; } //id �����

        [Required]
        [StringLength(50)]
        public string UId //id ���������
        {
            get { return _uId; }
            set { _uId = value; OnPropertyChanged(); }
        }

        public DateTime StartDateTime { get; private set; } //���� � ����� ������ �����

        public DateTime? EndDateTime { get; private set; } //���� � ����� ����� �����

        public decimal CurrentCash //������� ������
        {
            get { return _currentCash; }
            set { _currentCash = value; OnPropertyChanged(); }
        }

        public decimal CashReceived //����� ������
        {
            get { return _cashReceived; }
            set { _cashReceived = value; OnPropertyChanged(); }
        }

        public decimal CashReturned //����� ���������
        {
            get { return _cashReturned; }
            set { _cashReturned = value; OnPropertyChanged(); }
        }

        public decimal CashAdded //����� ��������
        {
            get { return _cashAdded; }
            set { _cashAdded = value; OnPropertyChanged(); }
        }

        public decimal CashWithdrawn //����� �������
        {
            get { return _cashWithdrawn; }
            set { _cashWithdrawn = value; OnPropertyChanged(); }
        }

        #endregion
        #region Methods
        public static async void AddMoneyAsync(decimal money, Shift shift) //�������� �������
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
                    TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs("�������� ������� ���������!", true));
                }
                catch (Exception e)
                {
                    TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs(e.Message + "\n�������� ��������� �� ����!", false));
                }
            }
        }
        public static async void WithdrawMoneyAsync(decimal money, Shift shift) //������� �������
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
                        TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs("�������� ������� ������!", true));
                    }
                    else
                    {
                        TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs("���������� ������ ��������� �����, ��� ��� � ����� ��������� ������ �������, ��� ���������", false));
                    }
                }
                catch (Exception e)
                {
                    TransactionCompleted?.Invoke(shift, new ShiftTransactionEventArgs(e.Message + "\n�������� ������ �� ����!", false));
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public static Shift ShiftStart(IUser<User> user) //����� �����
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
        public bool EndShift() //���������� �����
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
