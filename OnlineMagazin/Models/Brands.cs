
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMagazin.Models
{
    [Table("Brands")]
    public class Brands
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandsId { get; set; }

        [StringLength(800)]
        [DisplayName("Выберите фото")]
        public string BrandsResim { get; set; }

        [StringLength(100)]
        [DisplayName("Название бренда")]
        public string BrandsName { get; set; }

        [NotMapped]
        [DisplayName("Выберите фото")]
        public IFormFile BrandsResimFile { get; set; }
    }
}