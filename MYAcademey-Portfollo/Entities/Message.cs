using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class Message
    {
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad soyad en fazla 100 karakter olabilir.")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string SenderMail { get; set; }

        [Required(ErrorMessage = "Konu zorunludur.")]
        [StringLength(150, ErrorMessage = "Konu en fazla 150 karakter olabilir.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Mesaj zorunludur.")]
        [StringLength(3000, ErrorMessage = "Mesaj en fazla 3000 karakter olabilir.")]
        public string MessageDetail { get; set; }

        public DateTime SendDate { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}