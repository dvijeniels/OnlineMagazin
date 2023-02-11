using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMagazin.Models
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
        public int ProductId { get; set; }

        [StringLength(255)]
        [DisplayName("Значение")]
        [Required(ErrorMessage = "Пожалуйста введите значение")]
        public string Value { get; set; }

        //[StringLength(255)]
        //[DisplayName("Цена для этого значенмя")]
        //[Required(ErrorMessage = "Пожалуйста введите цену для этого значения")]
        //public string newPrice { get; set; }

        [DisplayName("Категория")]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [DisplayName("Характеристика товара")]
        [ForeignKey("CategoryFeatureId")]
        public virtual CategoryFeature CategoryFeature { get; set; }

        [DisplayName("Товар")]
        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }

    }
}