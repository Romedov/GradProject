using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradProject.Models
{
    public class SignInEventArgs : EventArgs
    {
        #region Fields
        #endregion
        #region Properties
        public bool IsSuccessful { get; private set; }
        #endregion
        #region Constructors
        public SignInEventArgs(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
        #endregion
    }
}
