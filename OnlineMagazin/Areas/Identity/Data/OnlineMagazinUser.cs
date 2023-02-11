using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Models;

namespace OnlineMagazin.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the OnlineMagazinUser class
    public class OnlineMagazinUser : IdentityUser
    {
        [StringLength(30)]
        [DisplayName("ФИО")]
        public string FirstAndLastName { get; set; }

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

        [StringLength(250)]
        [DisplayName("Выберите фото")]
        public string Foto { get; set; }

        [NotMapped]
        [DisplayName("Выберите фото")]
        public IFormFile ResimFile { get; set; }

        //[DisplayName("Роль пользователя")]
        //public int RoleId { get; set; }

        public virtual ICollection<Orders> Order { get; set; }
    }
}
