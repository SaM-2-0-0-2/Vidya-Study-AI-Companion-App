using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vidya_AI_Study_Companion.Models
{
    public class StudyMaterial
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Resource { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TagId { get; set; }

        public User User { get; set; }
        public Tag Tag { get; set; }
    }
}
