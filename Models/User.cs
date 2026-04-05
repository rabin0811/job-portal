using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "JobSeeker"; // JobSeeker | Employer | Admin

        public string? ProfilePhotoPath { get; set; }
        public string? Headline { get; set; }
        public string? Location { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        public virtual UserProfile? UserProfile { get; set; }
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
