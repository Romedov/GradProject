using System;

namespace GradProject.Models
{
    public class ItemSearchEventArgs: EventArgs 
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
