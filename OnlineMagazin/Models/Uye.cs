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
        [Required(ErrorMessage = "Пожалуйста введите логин")]
        [DisplayName("Логин")]
        public string KullaniciAdi { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Пожалуйста введите почту")]
        [EmailAddress(ErrorMessage = "Поле электронной почты не является действительным адресом электронной почты.")]
        [DisplayName("Почта")]
        public string Email { get; set; }

        [StringLength(20)]
        [DisplayName("Номер телефон")]
        public string PhoneNumber { get; set; }

        [StringLength(30)]
        [DisplayName("Область")]
        public string Region { get; set; }

        [StringLength(100)]
        [DisplayName("Город")]
        public string City { get; set; }

        [StringLength(500)]
        [DisplayName("Адрес 1")]
        public string Address { get; set; }

        [StringLength(500)]
        [DisplayName("Адрес 2")]
        public string Address2 { get; set; }

        [StringLength(1000)]
        [DisplayName("Дополнительная информация")]
        public string Description { get; set; }

        [DisplayName("Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime BirthDate { get; set; }

        [StringLength(20, ErrorMessage = "Должно быть как минимум 6 символов.", MinimumLength = 6)]
        [Required(ErrorMessage = "Пожалуйста введите пароль")]
        [DisplayName("Пароль")]
        public string Sifre { get; set; }

        [NotMapped]
        [DisplayName("Подтвердить пароль")]
        [Compare("Sifre", ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Пожалуйста введите ФИО")]
        [DisplayName("ФИО")]
        public string AdSoyad { get; set; }

        [StringLength(250)]
        [DisplayName("Выберите фото")]
        public string Foto { get; set; }

        [NotMapped]
        [DisplayName("Выберите фото")]
        public IFormFile ResimFile { get; set; }

        public virtual ICollection<Orders> Order { get; set; }

        public virtual ICollection<Carts> Carts { get; set; }

        [DisplayName("Админ")]
        public bool Yetki { get; set; }
    }
}