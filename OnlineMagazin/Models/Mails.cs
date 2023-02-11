namespace OnlineMagazin.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Mails")]
    public partial class Mails
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MailId { get; set; }

        [StringLength(80)]
        [Required(ErrorMessage = "���������� ������� � �����")]
        [DisplayName("�����")]
        public string Mail { get; set; }

        [DisplayName("������")]
        public bool Status { get; set; }
    }
}
