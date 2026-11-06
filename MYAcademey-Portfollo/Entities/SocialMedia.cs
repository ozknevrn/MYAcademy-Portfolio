using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class SocialMedia
    {
        public int SocialMediaId { get; set; }

        [Required(ErrorMessage = "Platform adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Platform adı en fazla 50 karakter olabilir.")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "Bağlantı zorunludur.")]
        [Url(ErrorMessage = "Geçerli bir bağlantı giriniz.")]
        public string Url { get; set; }

        [StringLength(50)]
        public string? IconClass { get; set; }
    }
}