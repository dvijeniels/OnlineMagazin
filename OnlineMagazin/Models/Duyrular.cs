namespace OnlineMagazin.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Duyrular")]
    public partial class Duyrular
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DuyuruId { get; set; }

        [StringLength(100)]
        [DisplayName("��������")]
        [Required(ErrorMessage = "���������� ������� ��������")]
        public string DuyuruAd { get; set; }

        [StringLength(2000)]
        [DisplayName("�����")]
        [Required(ErrorMessage = "���������� ������� �����")]
        public string Duyuruicerik { get; set; }

        [StringLength(200)]
        [DisplayName("��������")]
        public string DuyuruResim { get; set; }

        [DisplayName("����")]
        public DateTime DuyuruDate { get; set; }

        [NotMapped]
        [DisplayName("�������� ��������")]
        public IFormFile DuyuruResimFile { get; set; }
    }
}
