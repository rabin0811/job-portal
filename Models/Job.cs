using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required] public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Location { get; set; }
        public string? JobType { get; set; } // Full-time, Part-time, Remote, Contract
        public string? Salary { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime? Deadline { get; set; }
        public bool IsActive { get; set; } = true;

        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        public int PostedByUserId { get; set; }
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
