using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMagazin.Models
{
    [Table("Orders")]
    public class Orders
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [DisplayName("Номер корзины")]
        public int CartId { get; set; }
        [DisplayName("Клиент")]
        public int UyeId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Пожалуйста введите ФИО")]
        [DisplayName("ФИО")]
        public string AdSoyad { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Пожалуйста введите номер телефона")]
        [DisplayName("Номер телефон")]
        public int NumberPhone { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Пожалуйста введите адрес")]
        [DisplayName("Адрес")]
        public string Address { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Пожалуйста введите статус заказа")]
        [DisplayName("Статус заказа")]
        public string Status { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Пожалуйста введите вид оплаты")]
        [DisplayName("Вид оплаты")]
        public string BuyingType { get; set; }

        [StringLength(20)]
        [DisplayName("Комментарии к товарам")]
        public string Comment { get; set; }

        [DisplayName("Дата создания заказа")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Дата прибытия к клиенту")]
        public DateTime? OrderDate { get; set; }
        public bool InOrder { get; set; }
        public Carts Cart { get; set; }
        public Uye Uye { get; set; }
    }
}