namespace OnlineMagazin.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Uye")]
    public partial class Uye
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UyeId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "���������� ������� �����")]
        [DisplayName("�����")]
        public string KullaniciAdi { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "���������� ������� �����")]
        [EmailAddress(ErrorMessage = "���� ����������� ����� �� �������� �������������� ������� ����������� �����.")]
        [DisplayName("�����")]
        public string Email { get; set; }

        [StringLength(20)]
        [DisplayName("����� �������")]
        public string PhoneNumber { get; set; }

        [StringLength(30)]
        [DisplayName("�������")]
        public string Region { get; set; }

        [StringLength(100)]
        [DisplayName("�����")]
        public string City { get; set; }

        [StringLength(500)]
        [DisplayName("����� 1")]
        public string Address { get; set; }

        [StringLength(500)]
        [DisplayName("����� 2")]
        public string Address2 { get; set; }

        [StringLength(1000)]
        [DisplayName("�������������� ����������")]
        public string Description { get; set; }

        [DisplayName("���� ��������")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime BirthDate { get; set; }

        [StringLength(20, ErrorMessage = "������ ���� ��� ������� 6 ��������.", MinimumLength = 6)]
        [Required(ErrorMessage = "���������� ������� ������")]
        [DisplayName("������")]
        public string Sifre { get; set; }

        [NotMapped]
        [DisplayName("����������� ������")]
        [Compare("Sifre", ErrorMessage = "������ �� ���������!")]
        public string ConfirmPassword { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "���������� ������� ���")]
        [DisplayName("���")]
        public string AdSoyad { get; set; }

        [StringLength(250)]
        [DisplayName("�������� ����")]
        public string Foto { get; set; }

        [NotMapped]
        [DisplayName("�������� ����")]
        public IFormFile ResimFile { get; set; }

        public virtual ICollection<Orders> Order { get; set; }

        public virtual ICollection<Carts> Carts { get; set; }

        [DisplayName("�����")]
        public bool Yetki { get; set; }
    }
}