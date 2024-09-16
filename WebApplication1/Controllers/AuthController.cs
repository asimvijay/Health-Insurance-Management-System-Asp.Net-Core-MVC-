using HealthInsurance.Entities;
using HealthInsurance.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HealthInsurance.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // User Account Actions
        public IActionResult Index()
        {
            return View(_context.Admin.ToList());
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            // Check if the user is in the Admin role
            if (User.IsInRole("Admin"))
            {
                // Redirect to /admin if the user is an Admin
                return Redirect("/admin");
            }

            // Optionally return a 404 page if not authorized
            return NotFound();
        }


        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeDashboard()
        {
            return View(); 
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            return Redirect("/");
        }

        // Admin Registration
        public IActionResult AdminRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminRegistration(AdminRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = new AdminAccount
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Role = Role.Admin
                };

                try
                {
                    _context.Admin.Add(admin);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();
                    ViewBag.Message = $"{admin.UserName} registered successfully.";
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while registering. Please ensure the username is unique.");
                    return View(model);
                }
            }
            return View(model);
        }

        // Employee Registration
        public IActionResult EmpRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmpRegistration(EmpRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new EmpRegister
                {
                    Designation = model.Designation,
                    JoinDate = model.JoinDate,
                    Salary = model.Salary,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Username = model.Username,
                    Password = model.Password,
                    Address = model.Address,
                    ContactNo = model.ContactNo,
                    State = model.State,
                    Country = model.Country,
                    City = model.City,
                    PolicyStatus = model.PolicyStatus,
                };

                try
                {
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();
                    ViewBag.Message = $"{employee.FirstName} {employee.LastName} registered successfully.";
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while registering. Please ensure the username is unique.");
                    return View(model);
                }
            }
            return View(model);
        }

        // Admin Login
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = await _context.Admin
                    .Where(x => x.UserName == model.UserName && x.Password == model.Password)
                    .FirstOrDefaultAsync();

                if (admin != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, admin.UserName),
                        new Claim(ClaimTypes.Role, "Admin")
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("AdminDashboard", "Auth");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect.");
                }
            }
            return View();
        }

        // Employee Login
        public IActionResult EmpLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmpLogin(EmpLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _context.Employees
                    .Where(x => x.Username == model.Username && x.Password == model.Password)
                    .FirstOrDefaultAsync();

                if (employee != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, employee.Username),
                        new Claim(ClaimTypes.Role, "Employee")
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("EmployeeDashboard", "Auth");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect.");
                }
            }
            return View();
        }

        [Authorize(Roles = "Employee")]
        public IActionResult SecurePageForEmp()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
