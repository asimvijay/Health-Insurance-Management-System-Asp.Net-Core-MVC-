using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Entities;
using HealthInsurance.Models;

namespace HealthInsurance.Controllers
{
    [Route("admin/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10; // Number of employees per page

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/employees
        [HttpGet("")]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            var employees = from e in _context.Employees.Include(e => e.Company)
                            select e;

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e =>
                    e.FirstName.Contains(searchString) ||
                    e.LastName.Contains(searchString) ||
                    e.Company.CompanyName.Contains(searchString));
            }

            var totalItems = await employees.CountAsync();
            var employeesToDisplay = await employees
                .OrderBy(e => e.EmpNo) // Or use a specific property for ordering
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var viewModel = new EmployeesIndexViewModel
            {
                Employees = employeesToDisplay,
                SearchString = searchString,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize)
            };

            return View(viewModel);
        }

        // GET: admin/employees/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empRegister = await _context.Employees
                .Include(e => e.Company)
                .FirstOrDefaultAsync(m => m.EmpNo == id);
            if (empRegister == null)
            {
                return NotFound();
            }

            return View(empRegister);
        }

        // GET: admin/employees/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View();
        }

        // POST: admin/employees/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Username,Password,Designation,JoinDate,Salary,Address,ContactNo,City,State,Country,PolicyStatus,CompanyId")] EmpRegisterDto empRegister)
        {
            if (ModelState.IsValid)
            {
                var employee = new EmpRegister
                {
                    FirstName = empRegister.FirstName,
                    LastName = empRegister.LastName,
                    Username = empRegister.Username,
                    Password = empRegister.Password,
                    Designation = empRegister.Designation,
                    JoinDate = empRegister.JoinDate,
                    Salary = empRegister.Salary,
                    Address = empRegister.Address,
                    ContactNo = empRegister.ContactNo,
                    City = empRegister.City,
                    State = empRegister.State,
                    Country = empRegister.Country,
                    PolicyStatus = empRegister.PolicyStatus,
                    CompanyId = empRegister.CompanyId
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", empRegister.CompanyId);
            return View(empRegister);
        }


        // GET: admin/employees/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empRegister = await _context.Employees.FindAsync(id);
            if (empRegister == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", empRegister.CompanyId);
            return View(empRegister);
        }

        // POST: admin/employees/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpNo,FirstName,LastName,Username,Password,Designation,JoinDate,Salary,Address,ContactNo,City,State,Country,PolicyStatus,CompanyId")] EmpRegister empRegister)
        {
            if (id != empRegister.EmpNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empRegister);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpRegisterExists(empRegister.EmpNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", empRegister.CompanyId);
            return View(empRegister);
        }

        // GET: admin/employees/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empRegister = await _context.Employees
                .Include(e => e.Company)
                .FirstOrDefaultAsync(m => m.EmpNo == id);
            if (empRegister == null)
            {
                return NotFound();
            }

            return View(empRegister);
        }

        // POST: admin/employees/delete/5
        [HttpPost("delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empRegister = await _context.Employees.FindAsync(id);
            if (empRegister == null)
            {
                return NotFound();
            }

            try
            {
                _context.Employees.Remove(empRegister);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Check if the exception is due to a foreign key constraint violation
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    ModelState.AddModelError("", "Unable to delete. This employee is referenced by existing records.");
                    return View("Delete", empRegister); // Return to the Delete view with an error message
                }

                // If it's a different issue, rethrow the exception
                throw;
            }
        }

        private bool EmpRegisterExists(int id)
        {
            return _context.Employees.Any(e => e.EmpNo == id);
        }
    }

    public class EmployeesIndexViewModel
    {
        public IEnumerable<EmpRegister> Employees { get; set; }
        public string SearchString { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
