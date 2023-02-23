using Microsoft.AspNetCore.Identity;
using OnlineMagazin.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMagazin.Models
{
    public class OrderDetails
    {
        public int OrderId { get; set; }

        [StringLength(12)]
        [DisplayName("Номер заказа")]
        public string OrderNumber { get; set; }

        [DisplayName("Клиент")]
        public string UserId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Пожалуйста введите почту")]
        [DisplayName("Почта")]
        public string Email { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Пожалуйста введите ФИО")]
        [DisplayName("ФИО")]
        public string FirstAndLastName { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите адрес 1")]
        [StringLength(500)]
        [DisplayName("Адрес 1")]
        public string Address { get; set; }

        [StringLength(500)]
        [DisplayName("Адрес 2")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите телефон номер")]
        [StringLength(20)]
        [DisplayName("Номер телефон")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите область")]
        [StringLength(50)]
        [DisplayName("Область")]
        public string Region { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите город")]
        [StringLength(100)]
        [DisplayName("Город")]
        public string City { get; set; }

        [StringLength(30)]
        [DisplayName("Статус заказа")]
        public string Status { get; set; }

        [StringLength(20)]
        [DisplayName("Вид оплаты")]
        public string BuyingType { get; set; }

        [StringLength(2000)]
        [DisplayName("Дополнительно к заказу")]
        public string Comment { get; set; }

        [DisplayName("Дата создания заказа")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Дата отправки к клиенту")]
        public DateTime? OrderDate { get; set; }

        [DisplayName("Заказ выполнен ли?")]
        public bool InOrder { get; set; }
        public virtual OnlineMagazinUser OnlineMagazinUser { get; set; }
        public virtual List<OrderDetailLines> OrderDetailLines { get; set; }
    }
    public class OrderDetailLines
    {
        public int OrderId { get; set; }
        public virtual Orders Order { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        [DisplayName("Количество")]
        public ushort qty { get; set; }

        [DisplayName("Цена")]
        public double Price { get; set; }


        [StringLength(800)]
        [DisplayName("1-картинка")]
        public string Foto { get; set; }

        [StringLength(800)]
        [DisplayName("2-картинка")]
        public string Foto2 { get; set; }
    }
}