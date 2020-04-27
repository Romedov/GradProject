using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.Interfaces
{
    public interface IShiftEventSender
    {
        decimal ID { get; }
        int UserID { get; }
        DateTime StartDateTime { get; }
        DateTime? EndDateTime { get; }
        decimal Balance { get; }
        decimal AddsSum { get; }
        decimal WithdrawalsSum { get; }
        decimal SalesSum { get; }
        decimal ReturnsSum { get; }
    }
}
