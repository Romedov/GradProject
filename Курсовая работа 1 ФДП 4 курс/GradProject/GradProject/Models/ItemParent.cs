using System;

namespace GradProject
{
    public abstract class ItemParent //родительский класс товаров
    {
        public abstract string IId { get; protected set; } //id

        public abstract string Name { get; protected set; } //наименование

        public abstract decimal Price { get; protected set; } //цена

        public abstract long Number { get; set; } //количество

        public abstract void SellItemAsync(Shift currShift); //продажа

        public abstract void ReturnItemAsync(Shift currShift); //возврат
    }
}
