using JobPortal.Filters;
using JobPortal.Models;
using JobPortal.Services.Abstraction;
using JobPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        private readonly IJobService _jobService;
        private readonly IWebHostEnvironment _env;

        public CompanyController(ICompanyService companyService, IUserService userService, IJobService jobService, IWebHostEnvironment env)
        {
            _companyService = companyService;
            _userService = userService;
            _jobService = jobService;
            _env = env;
        }

        public async Task<IActionResult> Index() => View(await _companyService.GetAllAsync());

        public async Task<IActionResult> Details(int id) => View(await _companyService.GetByIdAsync(id));

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(CompanyRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userService.GetByEmailAsync(model.Email);
                if (userExists != null)
                {
                    ModelState.AddModelError("Email", "Email already registered.");
                    return View(model);
                }

                string? logoPath = null;
                if (model.Logo != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(model.Logo.FileName);
                    var folder = Path.Combine(_env.WebRootPath, "Logos");
                    Directory.CreateDirectory(folder);
                    using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
                    await model.Logo.CopyToAsync(stream);
                    logoPath = "/Logos/" + fileName;
                }

                var company = new Company
                {
                    Name = model.CompanyName,
                    Location = model.Location,
                    Description = model.Description,
                    Website = model.Website,
                    LogoPath = logoPath
                };
                await _companyService.AddAsync(company);

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = "Employer",
                    CompanyId = company.Id
                };
                await _userService.UpdateAsync(user);

                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [RoleAuthorize("Employer")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var user = await _userService.GetByIdAsync(userId);
            if (user?.CompanyId == null) return RedirectToAction("Register");

            HttpContext.Session.SetInt32("CompanyId", user.CompanyId.Value);
            var jobs = await _jobService.GetJobsByCompanyAsync(user.CompanyId.Value);
            return View(jobs);
        }
    }
}
