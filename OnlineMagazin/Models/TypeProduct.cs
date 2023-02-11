namespace OnlineMagazin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TypeProduct")]
    public partial class TypeProduct
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Пожалуйста введите тип")]
        [DisplayName("Тип")]
        public string TypeName{ get; set; }

        public virtual ICollection<Products> Product { get; set; }

    }
}
