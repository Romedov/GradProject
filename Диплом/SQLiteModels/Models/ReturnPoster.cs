using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models
{
    public class ReturnPoster : IPoster
    {
        public bool TryPost(IPostable receipt, IShift shift, out string message, decimal money = 0)
        {
            if ((receipt as Receipt != null) && (shift as Shift != null))
            {
                if (this.CanPost(receipt, shift, out string innerMessage))
                {
                    using (CashRegisterContext ctx = new CashRegisterContext())
                    {
                        if (ctx.CheckConnection(out string dbMessage))
                        {
                            receipt.AssignShiftAndDate(shift);
                            shift.ChangeReturnsStats(receipt.TotalPrice);
                            ctx.Receipts.Add(receipt as Receipt);
                            ctx.Shifts.Update(shift as Shift);
                            ctx.SaveChanges();
                            message = innerMessage;
                            return true;
                        }
                        else
                        {
                            message = dbMessage;
                            return false;
                        }
                    }
                }
                else
                {
                    message = innerMessage;
                    return false;
                }
            }
            else
            {
                message = "не удалось получить ссылку на экземпляр смены, либо чека";
                return false;
            }

        }
        private bool CanPost(IPostable receipt, IShift shift, out string message)
        {
            if ((receipt.GetItemsCount() > 0) && (receipt.TotalPrice <= shift.Balance))
            {
                message = $"К возврату: {receipt.TotalPrice} руб.";
                return true;
            }
            else if (receipt.GetItemsCount() < 1)
            {
                message = $"список товаров пуст";
                return false;
            }
            else
            {
                message = $"недостаточно средств в кассе для выдачи сдачи " +
                    $"({receipt.TotalPrice - shift.Balance}).\n " +
                    $"В кассе: {shift.Balance} руб)";
                return false;
            }
        }
    }
}
