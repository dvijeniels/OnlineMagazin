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
        [Required(ErrorMessage = "���������� ������� ���")]
        [DisplayName("��� �����������")]
        public string MessageGonderen { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "���������� ������� ����")]
        [DisplayName("���� ���������")]
        public string MessageBaslik { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "���������� ������� �����")]
        [EmailAddress(ErrorMessage = "���� ����������� ����� �� �������� �������������� ������� ����������� �����.")]
        [DisplayName("����� �����������")]
        public string MessageMail { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "���������� ������� ���������")]
        [DisplayName("���������")]
        public string MessageIcerik { get; set; }

        [DisplayName("���� ���������")]
        public DateTime MessageDate { get; set; }

        [DisplayName("��������-��?")]
        public bool Read { get; set; }
    }
}
