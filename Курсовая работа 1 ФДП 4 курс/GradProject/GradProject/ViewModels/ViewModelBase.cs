using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GradProject.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged //базовый класс VM
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
