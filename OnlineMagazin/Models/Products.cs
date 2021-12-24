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
        [DisplayName("�������� ������")]
        [Required(ErrorMessage = "���������� ������� �������� ������")]
        public string Baslik { get; set; }

        [StringLength(5000)]
        [DisplayName("��������")]
        [Required(ErrorMessage = "���������� ������� ��������")]
        public string Icerik { get; set; }

        [StringLength(800)]
        [DisplayName("1-��������")]
        public string Foto { get; set; }

        [StringLength(800)]
        [DisplayName("2-��������")]
        public string Foto2 { get; set; }

        [StringLength(500)]
        [DisplayName("3-��������")]
        public string Foto3 { get; set; }

        [StringLength(500)]
        [DisplayName("4-��������")]
        public string Foto4 { get; set; }

        [StringLength(500)]
        [DisplayName("5-��������")]
        public string Foto5 { get; set; }

        [StringLength(500)]
        [DisplayName("6-��������")]
        public string Foto6 { get; set; }

        [NotMapped]
        [DisplayName("�������� �����������")]
        [Required(ErrorMessage = "���������� �������� ���� �� 2 �����������")]
        public List<IFormFile> FotoFile { get; set; }

        [DisplayName("���������")]
        [Required(ErrorMessage = "���������� �������� ���������")]
        public int? CategoryId { get; set; }

        [DisplayName("����")]
        [Required(ErrorMessage = "���������� ������� ����")]
        public double Price { get; set; }

        [DisplayName("���� ����������")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ProductDate { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Carts> Carts { get; set; }
        public ICollection<ProductFeatures> ProductFeatures { get; set; }

    }
}
