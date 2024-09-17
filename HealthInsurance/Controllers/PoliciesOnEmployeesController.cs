using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using HealthInsurance.Models;

namespace HealthInsurance.Controllers
{
    [Route("admin/[controller]")]
    public class PoliciesOnEmployeesController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10; // Number of policies per page

        public PoliciesOnEmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/policiesonemployees/index
        [HttpGet("")]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            var policies = from p in _context.PoliciesOnEmployees
                           .Include(p => p.Employee)
                           .Include(p => p.Policy)
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                policies = policies.Where(p =>
                    p.Employee.FirstName.Contains(searchString) ||
                    p.Policy.PolicyName.Contains(searchString));
            }

            var totalItems = await policies.CountAsync();
            var policiesToDisplay = await policies
                .OrderBy(p => p.Id) // Or use a specific property for ordering
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var viewModel = new PoliciesOnEmployeesIndexViewModel
            {
                PoliciesOnEmployees = policiesToDisplay,
                SearchString = searchString,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize)
            };

            return View(viewModel);
        }

        // GET: admin/policiesonemployees/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policiesOnEmployees = await _context.PoliciesOnEmployees
                .Include(p => p.Employee)
                .Include(p => p.Policy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policiesOnEmployees == null)
            {
                return NotFound();
            }

            return View(policiesOnEmployees);
        }

        // GET: admin/policiesonemployees/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "FirstName");
            ViewData["PolicyId"] = new SelectList(_context.Policies, "PolicyId", "PolicyName");
            return View();
        }

        // POST: admin/policiesonemployees/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpNo,PolicyId,StartDate,EndDate,EMI")] PoliciesOnEmployeesDto policiesOnEmployeesDto)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Create a new PoliciesOnEmployees entity and map values from the DTO
                var newPolicyOnEmployees = new PoliciesOnEmployees
                {
                    EmpNo = policiesOnEmployeesDto.EmpNo,
                    PolicyId = policiesOnEmployeesDto.PolicyId,
                    StartDate = policiesOnEmployeesDto.StartDate,
                    EndDate = policiesOnEmployeesDto.EndDate,
                    EMI = policiesOnEmployeesDto.EMI
                };

                // Add the new entity to the context
                _context.PoliciesOnEmployees.Add(newPolicyOnEmployees);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Redirect to the Index action
                return RedirectToAction(nameof(Index));
            }

            // If model state is not valid, populate the ViewData with existing data for the dropdown lists
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "FirstName", policiesOnEmployeesDto.EmpNo);
            ViewData["PolicyId"] = new SelectList(_context.Policies, "PolicyId", "PolicyName", policiesOnEmployeesDto.PolicyId);

            // Return the view with the current model state
            return View(policiesOnEmployeesDto);
        }


        // GET: admin/policiesonemployees/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policiesOnEmployees = await _context.PoliciesOnEmployees.FindAsync(id);
            if (policiesOnEmployees == null)
            {
                return NotFound();
            }
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "FirstName", policiesOnEmployees.EmpNo);
            ViewData["PolicyId"] = new SelectList(_context.Policies, "PolicyId", "PolicyName", policiesOnEmployees.PolicyId);
            return View(policiesOnEmployees);
        }

        // POST: admin/policiesonemployees/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmpNo,PolicyId,StartDate,EndDate,EMI")] PoliciesOnEmployeesDto policiesOnEmployees)
        {
            if (id != policiesOnEmployees.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPolicyOnEmployees = await _context.PoliciesOnEmployees.FindAsync(id);

                    if (existingPolicyOnEmployees == null)
                    {
                        return NotFound();
                    }

                    existingPolicyOnEmployees.EmpNo = policiesOnEmployees.EmpNo;
                    existingPolicyOnEmployees.PolicyId = policiesOnEmployees.PolicyId;
                    existingPolicyOnEmployees.StartDate = policiesOnEmployees.StartDate;
                    existingPolicyOnEmployees.EndDate = policiesOnEmployees.EndDate;
                    existingPolicyOnEmployees.EMI = policiesOnEmployees.EMI;

                    _context.PoliciesOnEmployees.Update(existingPolicyOnEmployees);
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PoliciesOnEmployeesExists(policiesOnEmployees.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "FirstName", policiesOnEmployees.EmpNo);
            ViewData["PolicyId"] = new SelectList(_context.Policies, "PolicyId", "PolicyName", policiesOnEmployees.PolicyId);
            return View(policiesOnEmployees);
        }



        // GET: admin/policiesonemployees/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policiesOnEmployees = await _context.PoliciesOnEmployees
                .Include(p => p.Employee)
                .Include(p => p.Policy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policiesOnEmployees == null)
            {
                return NotFound();
            }

            return View(policiesOnEmployees);
        }

        // POST: admin/policiesonemployees/delete/5
        [HttpPost("delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policiesOnEmployees = await _context.PoliciesOnEmployees.FindAsync(id);
            if (policiesOnEmployees == null)
            {
                return NotFound();
            }

            try
            {
                _context.PoliciesOnEmployees.Remove(policiesOnEmployees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Check if the exception is due to a foreign key constraint violation
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    ModelState.AddModelError("", "Unable to delete. This policy is referenced by existing records.");
                    return View("Delete", policiesOnEmployees); // Return to the Delete view with an error message
                }

                // If it's a different issue, rethrow the exception
                throw;
            }
        }

        private bool PoliciesOnEmployeesExists(int id)
        {
            return _context.PoliciesOnEmployees.Any(e => e.Id == id);
        }
    }

    public class PoliciesOnEmployeesIndexViewModel
    {
        public IEnumerable<PoliciesOnEmployees> PoliciesOnEmployees { get; set; }
        public string SearchString { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
