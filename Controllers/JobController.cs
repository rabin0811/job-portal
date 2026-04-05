using JobPortal.Filters;
using JobPortal.Models;
using JobPortal.Services.Abstraction;
using JobPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly ICompanyService _companyService;

        public JobController(IJobService jobService, ICompanyService companyService)
        {
            _jobService = jobService;
            _companyService = companyService;
        }

        public async Task<IActionResult> Index(string search, string type, string loc)
        {
            var jobs = await _jobService.GetAllJobsAsync(search, type, loc);
            ViewBag.Search = search;
            ViewBag.Type = type;
            ViewBag.Loc = loc;
            return View(jobs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            return View(job);
        }

        [RoleAuthorize("Employer")]
        public async Task<IActionResult> Create()
        {
            var companyId = HttpContext.Session.GetInt32("CompanyId");
            if (companyId == null) return RedirectToAction("Dashboard", "Company");
            return View(new JobCreateViewModel { CompanyId = companyId.Value });
        }

        [HttpPost]
        [RoleAuthorize("Employer")]
        public async Task<IActionResult> Create(JobCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = new Job
                {
                    Title = model.Title,
                    Description = model.Description,
                    Requirements = model.Requirements,
                    Location = model.Location,
                    JobType = model.JobType,
                    Salary = model.Salary,
                    Deadline = model.Deadline,
                    CompanyId = model.CompanyId,
                    PostedByUserId = HttpContext.Session.GetInt32("UserId") ?? 0
                };
                await _jobService.AddJobAsync(job);
                return RedirectToAction("Dashboard", "Company");
            }
            return View(model);
        }

        [HttpPost]
        [RoleAuthorize("Employer", "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _jobService.DeleteJobAsync(id);
            return RedirectToAction("Index");
        }
    }
}
