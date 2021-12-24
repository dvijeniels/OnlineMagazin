namespace OnlineMagazin.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Duyrular")]
    public partial class Duyrular
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DuyuruId { get; set; }

        [StringLength(100)]
        [DisplayName("Название")]
        [Required(ErrorMessage = "Пожалуйста введите название")]
        public string DuyuruAd { get; set; }

        [StringLength(2000)]
        [DisplayName("Текст")]
        [Required(ErrorMessage = "Пожалуйста введите текст")]
        public string Duyuruicerik { get; set; }

        [StringLength(200)]
        [DisplayName("Картинка")]
        public string DuyuruResim { get; set; }

        [DisplayName("Дата")]
        public DateTime DuyuruDate { get; set; }

        [NotMapped]
        [DisplayName("Выберите картинку")]
        public IFormFile DuyuruResimFile { get; set; }
    }
}
