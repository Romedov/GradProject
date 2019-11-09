using System.Collections.ObjectModel;
using System.Linq;

namespace GradProject
{
    public static class ObservableCollectionExtension
    {
        public static decimal Conclude<T>(this ObservableCollection<T> items) where T : ItemParent //метод расширения для ObservableCollection<ParentItem>
        {
            return items.Sum(i => i.Price * i.Number);
        }
    }
}
