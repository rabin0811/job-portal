using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public string? Skills { get; set; }
        public string? Experience { get; set; }
        public string? Education { get; set; }
        public string? About { get; set; }
        public string? CVPath { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
    }
}
