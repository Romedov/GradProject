using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.Interfaces
{
    public interface IPoster
    {
        bool TryPost(IPostable receipt, IShift shift, out string message, decimal money = 0);
    }
}
