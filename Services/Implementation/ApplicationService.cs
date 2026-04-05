using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Services.Implementation
{
    public class ApplicationService : IApplicationService
    {
        private readonly JobDbContext _context;

        public ApplicationService(JobDbContext context) => _context = context;

        public async Task<IEnumerable<Application>> GetAllAsync() =>
            await _context.Applications.Include(a => a.Job).ThenInclude(j => j!.Company).Include(a => a.User).OrderByDescending(a => a.ApplicationDate).ToListAsync();

        public async Task AddAsync(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Application>> GetByUserIdAsync(int userId) =>
            await _context.Applications.Where(a => a.UserId == userId).Include(a => a.Job).ThenInclude(j => j!.Company).OrderByDescending(a => a.ApplicationDate).ToListAsync();

        public async Task<IEnumerable<Application>> GetByJobIdAsync(int jobId) =>
            await _context.Applications.Where(a => a.JobId == jobId).Include(a => a.User).ThenInclude(u => u!.UserProfile).OrderByDescending(a => a.ApplicationDate).ToListAsync();

        public async Task<bool> ExistsAsync(int jobId, int userId) =>
            await _context.Applications.AnyAsync(a => a.JobId == jobId && a.UserId == userId);

        public async Task<Application?> GetByIdAsync(int id) =>
            await _context.Applications.Include(a => a.Job).Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);

        public async Task UpdateStatusAsync(int applicationId, ApplicationStatus status)
        {
            var app = await _context.Applications.FindAsync(applicationId);
            if (app != null)
            {
                app.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
