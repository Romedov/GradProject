﻿using Kassa.Models.EventsArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models.Interfaces
{
    public interface IShift
    {
        #region Events
        event Action<IShiftEventSender, ShiftStartedEventArgs> ShiftStarted;
        event Action<IShiftEventSender, ShiftFinishedEventArgs> ShiftFinished;
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
        void Sell(IPostable receipt, decimal money);
        void Return(IPostable receipt);
        Task AddMoney(decimal amount);
        Task WithdrawMoney(decimal amount);
        #endregion
    }
}
