using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradProject
{
    public abstract class ItemParent
    {
        public abstract string IId { get; protected set; }

        public abstract string Name { get; protected set; }

        public abstract decimal Price { get; protected set; }

        public abstract long Number { get; set; }

        public abstract void SellItem(Shift currShift);
    }
}
