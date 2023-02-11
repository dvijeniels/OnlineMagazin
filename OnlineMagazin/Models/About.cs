namespace OnlineMagazin.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("About")]
    public partial class About
    {

        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(3000)]
        [Required(ErrorMessage = "Пожалуйста введите о текст")]
        [DisplayName("Текст о себе")]
        public string MetinIcerik { get; set; }

        [StringLength(300)]
        [DisplayName("Фото")]
        public string Resim { get; set; }

        [NotMapped]
        [DisplayName("Выберите фото")]
        public IFormFile ResimFile { get; set; }
    }
}
