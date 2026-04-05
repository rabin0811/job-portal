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

        public async Task<IActionResult> Index(string? search, string? type, string? loc, int page = 1)
        {
            const int pageSize = 10;
            var result = await _jobService.GetAllJobsAsync(search, type, loc, page, pageSize);
            
            ViewBag.Search = search;
            ViewBag.Type = type;
            ViewBag.Loc = loc;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);
            
            return View(result.Jobs);
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

        [RoleAuthorize("Employer")]
        public async Task<IActionResult> Edit(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();

            var model = new JobUpdateViewModel
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Requirements = job.Requirements,
                Location = job.Location,
                JobType = job.JobType,
                Salary = job.Salary,
                Deadline = job.Deadline,
                CompanyId = job.CompanyId
            };
            return View(model);
        }

        [HttpPost]
        [RoleAuthorize("Employer")]
        public async Task<IActionResult> Edit(JobUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = await _jobService.GetJobByIdAsync(model.Id);
                if (job == null) return NotFound();

                job.Title = model.Title;
                job.Description = model.Description;
                job.Requirements = model.Requirements;
                job.Location = model.Location;
                job.JobType = model.JobType;
                job.Salary = model.Salary;
                job.Deadline = model.Deadline;

                await _jobService.UpdateJobAsync(job);
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
