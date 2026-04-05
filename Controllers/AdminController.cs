using JobPortal.Data;
using JobPortal.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly JobDbContext _context;

        public AdminController(JobDbContext context) => _context = context;

        [RoleAuthorize("Admin")]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.Jobs = await _context.Jobs.Include(j => j.Company).OrderByDescending(j => j.IssueDate).ToListAsync();
            ViewBag.Users = await _context.Users.Include(u => u.Company).ToListAsync();
            ViewBag.Companies = await _context.Companies.ToListAsync();
            ViewBag.Applications = await _context.Applications.CountAsync();
            return View();
        }

        [RoleAuthorize("Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.Role != "Admin") { _context.Users.Remove(user); await _context.SaveChangesAsync(); }
            TempData["Success"] = "User deleted.";
            return RedirectToAction("Dashboard");
        }

        [RoleAuthorize("Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null) { _context.Jobs.Remove(job); await _context.SaveChangesAsync(); }
            TempData["Success"] = "Job deleted.";
            return RedirectToAction("Dashboard");
        }

        [RoleAuthorize("Admin")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null) { _context.Companies.Remove(company); await _context.SaveChangesAsync(); }
            TempData["Success"] = "Company deleted.";
            return RedirectToAction("Dashboard");
        }

        [RoleAuthorize("Admin")]
        public async Task<IActionResult> PromoteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.Role == "JobSeeker") { user.Role = "Employer"; await _context.SaveChangesAsync(); }
            TempData["Success"] = "User promoted to Employer.";
            return RedirectToAction("Dashboard");
        }
    }
}
