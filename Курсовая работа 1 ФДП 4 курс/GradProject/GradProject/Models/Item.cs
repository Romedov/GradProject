using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Runtime.CompilerServices;
using System.Windows;
using GradProject.Models;
namespace GradProject
{
    public partial class Item : ItemParent, INotifyPropertyChanged
    {
        #region Constructors
        public Item() { }
        public Item(decimal price)
        {
            IId = "free";
            Price = price;
            Number = 1;
        }
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public static event EventHandler<ItemSearchEventArgs> ItemSearching;
        #endregion
        #region Private fields
        [NotMapped]
        private long _number;
        #endregion
        #region Public properties
        [Key]
        [StringLength(50)]
        public override string IId{ get; protected set; }

        [Required]
        [StringLength(100)]
        public override string Name { get; protected set; }

        public override decimal Price { get; protected set; }

        public override long Number
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged(); }
        }

        public int Discount { get; private set; }

        #endregion
        #region Methods
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public static Item GetItem(object sender, string iId)
        {
            using (CashboxDataContext db = new CashboxDataContext())
            {
                Item item = null;
                try
                {
                    db.DBConnectionCheck();
                    item = db.Items.FirstOrDefault(i => i.IId == iId);
                    /*
                    if (item.Discount != 0)
                    {
                        item.Price *= 1 - item.Discount / 100;
                    }
                    else if (item.Discount >= 100)
                    {
                        item.Price = 0;
                    }
                    */
                    if (item != null)
                    {
                        item.Number = 1;
                        ItemSearching?.Invoke(sender, new ItemSearchEventArgs($"“овар с идентификатором {iId} добавлен в чек!", true));
                        return item;
                    }
                    else
                    {
                        ItemSearching?.Invoke(sender, new ItemSearchEventArgs($"“овар с идентификатором {iId} в базе данных не зарегистрирован!", false));
                        return null;
                    }

                }
                catch (Exception e)
                {
                    ItemSearching?.Invoke(sender, new ItemSearchEventArgs($"Ќе удалось найти товар в базе данных!\n{e.Message}", false));
                    return null;
                }
            }
        }

        public override void SellItem(Shift currShift)
        {
            
        }
        //public bool SellItem()
        //{
        //    try
        //    {
        //        Item itemSold = null;
        //        using(CashboxModel db = new CashboxModel())
        //        {
        //            db.DBConnectionCheck();
        //            long currShiftId = db.Shifts.OrderByDescending(sh => sh.SId).Select(sh => sh.SId).FirstOrDefault();
        //            //проверка на существование данного ключа товара в текущей смене
        //            //добавление данного ключа при отсутствии, либо увеличение численности при наличии
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}
        #endregion
    }
}
