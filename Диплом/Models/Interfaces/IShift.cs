using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models.Interfaces
{
    public interface IShift
    {
        #region Events

        #endregion
        #region Public props
        decimal ID { get; }
        int UserID { get; }
        DateTime StartDateTime { get; }
        DateTime? EndDateTime { get; }
        decimal Balance { get; }
        decimal AddsSum { get; }
        decimal WithdrawalsSum { get; }
        decimal SalesSum { get; }
        decimal ReturnsSum { get; }
        #endregion
        #region Methods
        bool TryRestoreLastUnfinished(out IShift shift);
        void Start(IUser user);
        void Finish();
        void Sell(Receipt receipt);
        void Return(Receipt receipt);
        Task AddMoney(decimal amount);
        Task WithdrawMoney(decimal amount);
        #endregion
    }
}
