
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMagazin.Models
{
    [Table("Slider")]
    public class Sliders
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SliderId { get; set; }

        [StringLength(800)]
        [DisplayName("Выберите фото")]
        public string SliderResim { get; set; }

        [StringLength(100)]
        [DisplayName("Название продукта")]
        public string  SliderName { get; set; }

        [StringLength(100)]
        [DisplayName("Описание")]
        public string SliderDescription { get; set; }

        [NotMapped]
        [DisplayName("Выберите фото")]
        [Required(ErrorMessage = "Пожалуйста выберите фото")]
        public IFormFile SliderResimFile { get; set; }
    }
}