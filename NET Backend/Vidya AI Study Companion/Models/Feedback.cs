using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vidya_AI_Study_Companion.Models
{
    public class Feedback
    {
        [Key]
        public int FId { get; set; }

        [Required]
        [StringLength(60)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("Feedback")]
        public string FeedbackText { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
