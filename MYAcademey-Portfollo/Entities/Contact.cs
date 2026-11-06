using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    // Sitede gösterilen iletişim bilgileri (e-posta, telefon, konum).
    public class Contact
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "Konum en fazla 100 karakter olabilir.")]
        public string? Location { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [StringLength(30)]
        public string? PhoneNumber { get; set; }
    }
}
