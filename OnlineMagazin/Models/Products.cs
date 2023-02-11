namespace OnlineMagazin.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Products")]
    public class Products
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
        public List<IFormFile> FotoFile { get; set; }

        [DisplayName("Категория")]
        [Required(ErrorMessage = "Пожалуйста выберите категорию")]
        public int? CategoryId { get; set; }

        [DisplayName("Тип")]
        [Required(ErrorMessage = "Пожалуйста выберите тип")]
        public int? TypeId { get; set; }

        [DisplayName("Цена")]
        [Required(ErrorMessage = "Пожалуйста введите цену")]
        public double Price { get; set; }

        [DisplayName("Дата добавления")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ProductDate { get; set; }

        [DisplayName("Категория")]
        public virtual Category Category { get; set; }

        [DisplayName("Тип")]
        [ForeignKey("TypeId")]
        public virtual TypeProduct TypeProduct { get; set; }

        public virtual ICollection<ProductFeatures> ProductFeatures { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }

        public virtual ICollection<OrderLines> OrderLines { get; set; }

    }
}
