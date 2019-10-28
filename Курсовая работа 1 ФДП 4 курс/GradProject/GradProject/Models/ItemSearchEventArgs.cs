using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradProject.Models
{
    public class ItemSearchEventArgs
    {
        #region Fields
        #endregion
        #region Properties
        public bool IsSuccessful { get; private set; }
        public string Message { get; private set; }
        #endregion
        #region Constructors
        public ItemSearchEventArgs(string message, bool isSuccessful)
        {
            Message = message;
            IsSuccessful = isSuccessful;
        }
        #endregion
    }
}
