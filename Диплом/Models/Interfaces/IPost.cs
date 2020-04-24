using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.Models.Interfaces
{
    public interface IPost
    {
        bool TryPost(IShift shift, out string outMessage, decimal money);
    }
}
