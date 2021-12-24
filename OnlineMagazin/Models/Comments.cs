using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMagazin.Models
{
    [Table("Comments")]
    public class Comments
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [StringLength(500)]
        [DisplayName("Комментария")]
        [Required(ErrorMessage = "Пожалуйста оставьте комментарий")]
        public string Contents { get; set; }

        [DisplayName("Клиент")]
        public int? UyeId { get; set; }

        [DisplayName("Товар")]
        public int? ProductsId { get; set; }

        [DisplayName("Дата комментария")]
        public DateTime? Date { get; set; }

        public virtual Products Products { get; set; }

        public virtual Uye Uye { get; set; }
        
    }
}
