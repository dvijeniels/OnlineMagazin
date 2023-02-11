using OnlineMagazin.Areas.Identity.Data;
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

        [StringLength(80)]
        [DisplayName("Имя")]
        [Required(ErrorMessage = "Пожалуйста введите ваше имя")]
        public string UserName { get; set; }

        [DisplayName("Товар")]
        public int ProductId { get; set; }

        [DisplayName("Статус")]
        public bool Status { get; set; }

        [DisplayName("Оценка")]
        public int? Score { get; set; }

        [DisplayName("Дата комментария")]
        public DateTime? Date { get; set; }

        [DisplayName("Товар")]
        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }
    }
}
