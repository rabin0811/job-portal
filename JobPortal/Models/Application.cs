using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public enum ApplicationStatus
    {
        Pending,
        Reviewed,
        Accepted,
        Rejected
    }

    public class Application
    {
        public int Id { get; set; }

        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public string? Message { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    }
}
