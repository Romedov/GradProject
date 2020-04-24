using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models
{
    public class PostReturnReceipt : IPost
    {
        private readonly Receipt _receipt;
        public PostReturnReceipt (IPostable receipt)
        {
            this._receipt = receipt as Receipt;
        }
        public bool TryPost(IShift shift, out string outMessage, decimal money)
        {
            if (_receipt.CanPost(shift, out string message))
            {
                using (CashRegisterContext ctx = new CashRegisterContext())
                {
                    if (ctx.CheckConnection(out string dbMessage))
                    {
                        _receipt.AssignShiftAndDate(shift);
                        ctx.Receipts.Add(_receipt);
                        ctx.SaveChanges();
                        outMessage = message;
                        //totalPrice = _receipt.TotalPrice;
                        return true;
                    }
                    else
                    {
                        outMessage = dbMessage;
                        //totalPrice = 0;
                        return false;
                    }
                }
            }
            else
            {
                outMessage = message;
                //totalPrice = 0;
                return false;
            }
        }
    }
}
