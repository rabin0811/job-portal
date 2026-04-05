using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Services.Implementation
{
    public class JobService : IJobService
    {
        private readonly JobDbContext _context;

        public JobService(JobDbContext context) => _context = context;

        public async Task<(List<Job> Jobs, int TotalCount)> GetAllJobsAsync(string? search = null, string? jobType = null, string? location = null, int page = 1, int pageSize = 10)
        {
            var query = _context.Jobs.Include(j => j.Company).Where(j => j.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(j => j.Title!.Contains(search) || j.Description!.Contains(search));

            if (!string.IsNullOrEmpty(jobType))
                query = query.Where(j => j.JobType == jobType);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(j => j.Location!.Contains(location));

            int totalCount = await query.CountAsync();
            var jobs = await query.OrderByDescending(j => j.IssueDate)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return (jobs, totalCount);
        }

        public async Task<Job?> GetJobByIdAsync(int id) =>
            await _context.Jobs.Include(j => j.Company).Include(j => j.Applications).ThenInclude(a => a.User).FirstOrDefaultAsync(j => j.Id == id);

        public async Task AddJobAsync(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJobAsync(Job job)
        {
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Job>> GetJobsByCompanyAsync(int companyId) =>
            await _context.Jobs.Include(j => j.Applications).Where(j => j.CompanyId == companyId).OrderByDescending(j => j.IssueDate).ToListAsync();
    }
}
