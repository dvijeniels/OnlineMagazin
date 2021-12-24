
using OnlineMagazin.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    [Table("CategoryFeature")]
    public class CategoryFeature
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KatFId { get; set; }

        [Required(ErrorMessage = "Пожалуйста выберите категорию")]
        [DisplayName("Название категории")]
        public int CategoryId { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Пожалуйста введите название характеристики")]
        [DisplayName("Название характеристики")]
        public string FeatureName { get; set; }

        [StringLength(255)]
        [DisplayName("Единица измерения")]
        public string Unit { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<ProductFeatures> ProductFeatures { get; set; }
    }
}