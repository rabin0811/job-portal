using JobPortal.Models;
using JobPortal.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJobService _jobService;

        public HomeController(IJobService jobService) => _jobService = jobService;

        public async Task<IActionResult> Index()
        {
            var featuredJobs = await _jobService.GetAllJobsAsync();
            return View(featuredJobs.Take(6).ToList());
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
