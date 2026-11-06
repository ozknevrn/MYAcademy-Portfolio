using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur.")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter olabilir.")]
        public string ProjectTitle { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500)]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        [StringLength(200)]
        public string? Technologies { get; set; }   

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kategori seçiniz.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}