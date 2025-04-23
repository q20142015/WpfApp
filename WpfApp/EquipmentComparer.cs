using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    internal class EquipmentComparer : IComparer<Equipment>
    {
        public int Compare(Equipment? e1, Equipment? e2)
        {
            if (e1 is null || e2 is null)
                throw new ArgumentException("Некорректное значение параметра");
            return e1.Id - e2.Id;
        }
    }
}
