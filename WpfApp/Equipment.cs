using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    [Table("Оборудование")]
    public class Equipment
    {
        public int Id { get; set; }

        [Column("Название")]
        [Description("Название")]
        public string? Name { get; set; }

        [Column("Тип оборудования")]
        [Description("Тип оборудования")]
        public string? EquipmentType { get; set; }
    }
}
