using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class LoginViewModel
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    }

    public class CompanyRegisterViewModel
    {
        [Required] public string CompanyName { get; set; } = string.Empty;
        [Required] public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Website { get; set; }
        public IFormFile? Logo { get; set; }

        // Employer contact
        [Required] public string Name { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    }

    public class UserProfileViewModel
    {
        public string? Skills { get; set; }
        public string? Experience { get; set; }
        public string? Education { get; set; }
        public string? About { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? Headline { get; set; }
        public string? Location { get; set; }
        public IFormFile? CV { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
        // Read-only display
        public string? ExistingCVPath { get; set; }
        public string? ExistingPhotoPath { get; set; }
    }

    public class JobCreateViewModel
    {
        [Required] public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Location { get; set; }
        public string? JobType { get; set; }
        public string? Salary { get; set; }
        public DateTime? Deadline { get; set; }
        public int CompanyId { get; set; }
    }

    public class JobUpdateViewModel : JobCreateViewModel
    {
        public int Id { get; set; }
    }
}
