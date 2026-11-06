using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class HeroTitle
    {
        public int HeroTitleId { get; set; }

        [Required(ErrorMessage = "Unvan zorunludur.")]
        [StringLength(50, ErrorMessage = "Unvan en fazla 50 karakter olabilir.")]
        public string Title { get; set; }

        [Range(0, 100, ErrorMessage = "Sıra 0 ile 100 arasında olmalıdır.")]
        public int Order { get; set; }

        public bool IsActive { get; set; } = true;
    }
}