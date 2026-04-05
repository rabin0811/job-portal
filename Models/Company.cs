using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? LogoPath { get; set; }

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public virtual ICollection<User> Employees { get; set; } = new List<User>();
    }
}
