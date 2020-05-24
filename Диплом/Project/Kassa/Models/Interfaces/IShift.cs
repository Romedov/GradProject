using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kassa.Models.Interfaces
{
    public interface IShift
    {
        #region Public props
        long ID { get; }
        int UserID { get; }
        DateTime StartDateTime { get; }
        DateTime? EndDateTime { get; }
        decimal Balance { get; }
        decimal AddsSum { get; }
        decimal WithdrawalsSum { get; }
        decimal SalesSum { get; }
        decimal ReturnsSum { get; }
        ShiftState State { get; }

        User User { get; set; }
        IList<Receipt> Receipts { get; set; }
        IList<Supply> Supplies { get; set; }
        IList<Disposal> Disposals { get; set; }

        #endregion
        #region Methods
        Task Start(IUser user);
        Task Finish();
        void ChangeSalesStats(decimal money);
        void ChangeReturnsStats(decimal money);
        Task AddMoney(decimal amount);
        Task WithdrawMoney(decimal amount);
        #endregion
    }
}
