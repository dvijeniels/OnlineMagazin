namespace OnlineMagazin.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Message")]
    public partial class Message
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Пожалуйста введите ФИО")]
        [DisplayName("ФИО отправителя")]
        public string MessageGonderen { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Пожалуйста введите тему")]
        [DisplayName("Тема сообщения")]
        public string MessageBaslik { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Пожалуйста введите почту")]
        [EmailAddress(ErrorMessage = "Поле электронной почты не является действительным адресом электронной почты.")]
        [DisplayName("Почта отправителя")]
        public string MessageMail { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "Пожалуйста введите сообщение")]
        [DisplayName("Сообщение")]
        public string MessageIcerik { get; set; }

        [DisplayName("Дата сообщения")]
        public DateTime MessageDate { get; set; }

        [DisplayName("Прочитан-ли?")]
        public bool Read { get; set; }
    }
}
