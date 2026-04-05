using JobPortal.Models;
using JobPortal.Services.Abstraction;
using JobPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace JobPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService) => _userService = userService;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetByEmailAsync(model.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserRole", user.Role);
                    HttpContext.Session.SetString("UserName", user.Name);
                    
                    if (user.ProfilePhotoPath != null)
                        HttpContext.Session.SetString("UserPhoto", user.ProfilePhotoPath);

                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = "Invalid email or password.";
            }
            return View(model);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = await _userService.GetByEmailAsync(model.Email);
                if (existing != null)
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return View(model);
                }

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = "JobSeeker"
                };

                await _userService.AddAsync(user);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
