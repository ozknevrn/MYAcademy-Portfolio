using System.ComponentModel.DataAnnotations;

namespace MYAcademey_Portfollo.Entities
{
    public class SkillItem
    {
        public int SkillItemId { get; set; }

        [Required(ErrorMessage = "Yetenek adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Yetenek adı en fazla 50 karakter olabilir.")]
        public string SkillItemTitle { get; set; }

        [Range(0, 100, ErrorMessage = "Seviye 0 ile 100 arasında olmalıdır.")]
        public int Level { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir yetenek grubu seçiniz.")]
        public int SkillId { get; set; }

        public Skill? Skill { get; set; }
    }
}