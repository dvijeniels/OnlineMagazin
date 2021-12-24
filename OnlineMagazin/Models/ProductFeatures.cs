using OnlineMagazin.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    [Table("ProductFeatures")]
    public class ProductFeatures
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductFeatureId { get; set; }

        [DisplayName("Категория")]
        [Required(ErrorMessage = "Пожалуйста выберите категорие")]
        public int CategoryId { get; set; }

        [DisplayName("Характеристика")]
        [Required(ErrorMessage = "Пожалуйста выберите характеристику")]
        public int CategoryFeatureId { get; set; }

        [DisplayName("Товар")]
        [Required(ErrorMessage = "Пожалуйста выберите товар")]
        public int ProductsId { get; set; }

        [StringLength(255)]
        [DisplayName("Значение")]
        [Required(ErrorMessage = "Пожалуйста введите значение")]
        public string Value { get; set; }
        public virtual Category Category { get; set; }
        public virtual CategoryFeature CategoryFeature { get; set; }

        public virtual Products Products { get; set; }

    }
}