namespace OnlineMagazin.Models
{
    using OnlineShop.Models;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Products>();
        }
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Пожалуйста введите название категории")]
        [DisplayName("Название категории")]
        public string CategoriName{ get; set; }

        public virtual ICollection<Products> Products { get; set; }

        public virtual ICollection<CategoryFeature> CategoryFeature { get; set; }

        public virtual ICollection<ProductFeatures> ProductFeatures { get; set; }

    }
}
