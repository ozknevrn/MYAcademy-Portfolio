using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class Experience
    {
        public int ExperienceId { get; set; }

        [Required(ErrorMessage = "Deneyim başlığı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Şirket / kurum adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Şirket adı en fazla 100 karakter olabilir.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; }

        // Çalışma türü: Tam Zamanlı, Yarı Zamanlı, Uzaktan, Staj...
        [StringLength(50, ErrorMessage = "Çalışma türü en fazla 50 karakter olabilir.")]
        public string? CategoryName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
