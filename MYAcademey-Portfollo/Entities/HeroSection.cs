using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class HeroSection
    {
        public int HeroSectionId { get; set; }

        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        [StringLength(100)]
        public string Greeting { get; set; }

        [Required(ErrorMessage = "Unvan zorunludur.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Tanıtım yazısı zorunludur.")]
        [StringLength(1000)]
        public string Description { get; set; }

        public string? MainImageUrl { get; set; }

        // Öne çıkan bilgiler
        [StringLength(20)]
        public string? ExperienceYear { get; set; }    // örn: "10+"

        [StringLength(20)]
        public string? StudentCount { get; set; }      // örn: "200K+"
    }
}