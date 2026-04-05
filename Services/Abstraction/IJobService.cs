using JobPortal.Models;

namespace JobPortal.Services.Abstraction
{
    public interface IJobService
    {
        Task<List<Job>> GetAllJobsAsync(string? search = null, string? jobType = null, string? location = null);
        Task<Job?> GetJobByIdAsync(int id);
        Task AddJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(int id);
        Task<List<Job>> GetJobsByCompanyAsync(int companyId);
    }
}
