using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Services.Implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly JobDbContext _context;

        public CompanyService(JobDbContext context) => _context = context;

        public async Task<IEnumerable<Company>> GetAllAsync() =>
            await _context.Companies.Include(c => c.Jobs).ToListAsync();

        public async Task<Company?> GetByIdAsync(int id) =>
            await _context.Companies.Include(c => c.Jobs).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }
    }
}
