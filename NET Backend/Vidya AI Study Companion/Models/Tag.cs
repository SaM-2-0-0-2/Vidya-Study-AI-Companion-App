using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vidya_AI_Study_Companion.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required]
        [StringLength(50)]
        public string TagName { get; set; } = string.Empty;

        // Navigation Property (1 Tag → many StudyMaterials)
        public ICollection<StudyMaterial> StudyMaterials { get; set; } = new List<StudyMaterial>();
    }
}
