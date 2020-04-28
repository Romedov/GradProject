﻿using Kassa.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models
{
    public class SalePoster : IPoster
    {
        public bool TryPost(IPostable receipt, IShift shift, out string message, decimal money = 0)
        {
            if ((receipt as Receipt != null) && (shift as Shift != null))
            {
                if (this.CanPost(receipt, shift, money, out string innerMessage))
                {
                    using (CashRegisterContext ctx = new CashRegisterContext())
                    {
                        if (ctx.CanConnect)
                        {
                            receipt.AssignShiftAndDate(shift);
                            shift.ChangeSalesStats(receipt.TotalPrice);
                            ctx.Receipts.Add(receipt as Receipt);
                            ctx.Shifts.Update(shift as Shift);
                            ctx.SaveChanges();
                            message = innerMessage;
                            return true;
                        }
                        else
                        {
                            message = "нет соединения с базой данных";
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

        private bool CanPost(IPostable receipt, IShift shift, decimal money, out string message)
        {
            foreach(var item in receipt.Items)
            {
                if (!item.IsEnough(out float diff))
                {
                    message = $"Товара \"{item.ItemName}\" не хватает на складе ({diff} шт.)";
                    return false;
                }
                
            }

            if ((money >= receipt.TotalPrice) && (receipt.GetItemsCount() > 0) && ((money - receipt.TotalPrice) <= shift.Balance))
            {
                message = $"Сдача: {money - receipt.TotalPrice} руб.";
                return true;
            }
            else if (money < receipt.TotalPrice)
            {
                message = $"к оплате предоставлено недостаточно средств ({receipt.TotalPrice - money})";
                return false;
            }
            else if (receipt.GetItemsCount() < 1)
            {
                message = $"список товаров пуст";
                return false;
            }
            else
            {
                message = $"недостаточно средств для выдачи сдачи " +
                    $"({(money - receipt.TotalPrice) - shift.Balance}).\n " +
                    $"В кассе: {shift.Balance} руб)";
                return false;
            }
        }
    }
}
