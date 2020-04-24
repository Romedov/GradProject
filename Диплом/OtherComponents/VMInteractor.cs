using System;
using System.Collections.Generic;
using System.Text;
using Kassa.OtherComponents.Interfaces;
using Kassa.ViewModels;

namespace Kassa.OtherComponents
{
    public class VMInteractor : IVMInteractor
    {
        public VMInteractor()
        {

        }
        private readonly static List<ViewModelBase> _viewModels = new List<ViewModelBase>();
        public T GetViewModel<T>() where T : ViewModelBase
        {
            return _viewModels.Find(vm => vm.GetType() == typeof(T)) as T;
        }

        public void Register<T>(T vm) where T : ViewModelBase
        {
            _viewModels.Add(vm);
        }
    }
}
