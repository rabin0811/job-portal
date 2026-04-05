using JobPortal.Models;

namespace JobPortal.Services.Abstraction
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetAllAsync();
        Task AddAsync(Application application);
        Task<IEnumerable<Application>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Application>> GetByJobIdAsync(int jobId);
        Task<bool> ExistsAsync(int jobId, int userId);
        Task<Application?> GetByIdAsync(int id);
        Task UpdateStatusAsync(int applicationId, ApplicationStatus status);
    }
}
