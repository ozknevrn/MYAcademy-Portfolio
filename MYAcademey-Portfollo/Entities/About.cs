using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class About
    {
        public int AboutId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Hakkımda yazısı zorunludur.")]
        [StringLength(3000)]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}