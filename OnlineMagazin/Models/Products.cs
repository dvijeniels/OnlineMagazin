namespace OnlineMagazin.Models
{
    using Microsoft.AspNetCore.Http;
    using OnlineShop.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Products")]
    public partial class Products
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [StringLength(500)]
        [DisplayName("Название товара")]
        [Required(ErrorMessage = "Пожалуйста введите название товара")]
        public string Baslik { get; set; }

        [StringLength(5000)]
        [DisplayName("Описание")]
        [Required(ErrorMessage = "Пожалуйста введите описание")]
        public string Icerik { get; set; }

        [StringLength(800)]
        [DisplayName("1-картинка")]
        public string Foto { get; set; }

        [StringLength(800)]
        [DisplayName("2-картинка")]
        public string Foto2 { get; set; }

        [StringLength(500)]
        [DisplayName("3-картинка")]
        public string Foto3 { get; set; }

        [StringLength(500)]
        [DisplayName("4-картинка")]
        public string Foto4 { get; set; }

        [StringLength(500)]
        [DisplayName("5-картинка")]
        public string Foto5 { get; set; }

        [StringLength(500)]
        [DisplayName("6-картинка")]
        public string Foto6 { get; set; }

        [NotMapped]
        [DisplayName("Выберите изображение")]
        [Required(ErrorMessage = "Пожалуйста выберите хотя бы 2 изображений")]
        public List<IFormFile> FotoFile { get; set; }

        [DisplayName("Категория")]
        [Required(ErrorMessage = "Пожалуйста выберите категорию")]
        public int? CategoryId { get; set; }

        [DisplayName("Цена")]
        [Required(ErrorMessage = "Пожалуйста введите цену")]
        public double Price { get; set; }

        [DisplayName("Дата добавления")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ProductDate { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Carts> Carts { get; set; }
        public ICollection<ProductFeatures> ProductFeatures { get; set; }

    }
}
