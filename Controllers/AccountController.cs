using HealthInsurance.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HealthInsurance.Controllers
{
    public class AccountController : Controller
    {
        private readonly HealthInsuranceContext _context;

        public AccountController(HealthInsuranceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin login)
        {
            var user = await _context.UserLogins.FirstOrDefaultAsync(u => u.Username == login.Username && u.Password == login.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role,GetUserType(user.IsAdmin))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            if (user.IsAdmin == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("UserHomepage");
            }
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserLogin register, Employee employee)
        {
                _context.UserLogins.Add(register);
                _context.SaveChanges();

                employee.IdAccount = register.IdAccount;
                _context.Employees.Add(employee);
                _context.SaveChanges();

                return RedirectToAction("Login", "Account");
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "User")]
        public IActionResult UserHomepage()
        {
            return View();
        }

        private string GetUserType(byte userType)
        {
            switch (userType)
            {
                case 0:
                    return "User";
                case 1:
                    return "Admin";
                default:
                    return "";
            }
        }
    }
}
