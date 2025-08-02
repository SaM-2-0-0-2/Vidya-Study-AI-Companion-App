using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vidya_AI_Study_Companion.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(75)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(75)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; } = UserRole.USER;

        // Navigation Properties
        public ICollection<Summary> Summaries { get; set; } = new List<Summary>();
        public ICollection<StudyMaterial> StudyMaterials { get; set; } = new List<StudyMaterial>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }

    public enum UserRole
    {
        USER,
        TEACHER,
        ADMIN
    }
}
