using JobPortal.Models;

namespace JobPortal.Services.Abstraction
{
    public interface IJobService
    {
        Task<(List<Job> Jobs, int TotalCount)> GetAllJobsAsync(string? search = null, string? jobType = null, string? location = null, int page = 1, int pageSize = 10);
        Task<Job?> GetJobByIdAsync(int id);
        Task AddJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(int id);
        Task<List<Job>> GetJobsByCompanyAsync(int companyId);
    }
}
