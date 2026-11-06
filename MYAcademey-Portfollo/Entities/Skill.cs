using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    // Yetenek grubu (ör: Backend Geliştirme). İçindeki tek tek yetenekler SkillItem tablosunda tutulur.
    public class Skill
    {
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        // Bootstrap Icons sınıfı tutulur. Ör: "bi bi-server"
        public string? IconUrl { get; set; }

        public List<SkillItem>? SkillItems { get; set; }
    }
}
