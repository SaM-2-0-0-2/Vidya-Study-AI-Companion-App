using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vidya_AI_Study_Companion.Models
{
    public class Summary
    {
        [Key]
        public int SummaryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string SummaryEn { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string NativeLanguage { get; set; } = string.Empty;

        [Required]
        public string SummaryNt { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        // Navigation Property
        public User User { get; set; }
    }
}
