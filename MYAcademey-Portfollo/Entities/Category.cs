using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        public string CategoryName { get; set; }

        public List<Project>? Projects { get; set; }
    }
}