using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly JobDbContext _context;

        public UserService(JobDbContext context) => _context = context;

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.Include(u => u.Company).Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserProfile?> GetProfileAsync(int userId) =>
            await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

        public async Task SaveProfileAsync(UserProfile profile)
        {
            var existing = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == profile.UserId);
            if (existing == null)
                _context.UserProfiles.Add(profile);
            else
            {
                existing.Skills = profile.Skills;
                existing.Experience = profile.Experience;
                existing.Education = profile.Education;
                existing.About = profile.About;
                existing.CVPath = profile.CVPath ?? existing.CVPath;
                existing.LinkedInUrl = profile.LinkedInUrl;
                existing.GitHubUrl = profile.GitHubUrl;
                _context.UserProfiles.Update(existing);
            }
            await _context.SaveChangesAsync();
        }
    }
}
