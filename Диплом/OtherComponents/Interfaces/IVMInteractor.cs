using System;
using System.Collections.Generic;
using System.Text;

namespace Kassa.OtherComponents.Interfaces
{
    public interface IVMInteractor
    {
        T GetViewModel<T>() where T : ViewModels.ViewModelBase;
        void Register<T>(T vm) where T : ViewModels.ViewModelBase;

    }
}
