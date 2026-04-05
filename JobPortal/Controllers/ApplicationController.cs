using JobPortal.Filters;
using JobPortal.Models;
using JobPortal.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [RoleAuthorize("JobSeeker")]
        public IActionResult Apply(int jobId) => View(new Application { JobId = jobId });

        [HttpPost]
        [RoleAuthorize("JobSeeker")]
        public async Task<IActionResult> Apply(Application model)
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            if (await _applicationService.ExistsAsync(model.JobId, userId))
            {
                TempData["Error"] = "You have already applied for this job.";
                return RedirectToAction("Details", "Job", new { id = model.JobId });
            }

            model.UserId = userId;
            await _applicationService.AddAsync(model);
            TempData["Success"] = "Application submitted successfully!";
            return RedirectToAction("MyApplications");
        }

        [RoleAuthorize("JobSeeker")]
        public async Task<IActionResult> MyApplications()
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var apps = await _applicationService.GetByUserIdAsync(userId);
            return View(apps);
        }

        [RoleAuthorize("Employer")]
        public async Task<IActionResult> JobApplications(int jobId)
        {
            var apps = await _applicationService.GetByJobIdAsync(jobId);
            return View(apps);
        }

        [HttpPost]
        [RoleAuthorize("Employer")]
        public async Task<IActionResult> UpdateStatus(int id, ApplicationStatus status, int jobId)
        {
            await _applicationService.UpdateStatusAsync(id, status);
            return RedirectToAction("JobApplications", new { jobId });
        }
    }
}
