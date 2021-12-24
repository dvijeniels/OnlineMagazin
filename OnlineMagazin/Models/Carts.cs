using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMagazin.Models
{
    [Table("Carts")]
    public class Carts
    { 
        [Key]
        public int CartId { get; set; }
        [DisplayName("Товар")]
        public int ProductsId { get; set; }

        [DisplayName("Клиент")]
        public int? UyeId { get; set; }

        [DisplayName("Цена товара")]
        public double ProductPrice { get; set; }

        [DisplayName("Количество")]
        public ushort qty { get; set; }

        [DisplayName("Общая цена")]
        public double FinalPrice { get; set; }

        [DisplayName("Статус")]
        public bool InOrder { get; set; }
        public virtual Uye Uye { get; set; }
        public virtual Products Products { get; set; }
        public List<Orders> Orders { get; set; }
    }
}
