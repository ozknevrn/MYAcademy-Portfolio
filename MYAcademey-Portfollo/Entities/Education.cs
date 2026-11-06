using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class Education
    {
        public int EducationId { get; set; }

        [Required(ErrorMessage = "Bölüm / program adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir.")]
        public string Title { get; set; }

        [StringLength(150, ErrorMessage = "Okul adı en fazla 150 karakter olabilir.")]
        public string? SchoolName { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        // Kartın sağ üstündeki küçük etiket. Ör: "Lisans", "Mezuniyet", "Devam Ediyor"
        [StringLength(30, ErrorMessage = "Etiket en fazla 30 karakter olabilir.")]
        public string? BadgeText { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
