using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Entities;

namespace HealthInsurance.Controllers
{
    [Route("admin/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10; // Number of companies per page

        public CompaniesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/companies/index
        [HttpGet("")]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            var companies = from c in _context.Companies
                            select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                companies = companies.Where(c => c.CompanyName.Contains(searchString));
            }

            var totalItems = await companies.CountAsync();
            var companiesToDisplay = await companies
                .OrderBy(c => c.CompanyId)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var viewModel = new CompanyIndexViewModel
            {
                Companies = companiesToDisplay,
                SearchString = searchString,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize)
            };

            return View(viewModel);
        }

        // GET: admin/companies/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDetails = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (companyDetails == null)
            {
                return NotFound();
            }

            return View(companyDetails);
        }

        // GET: admin/companies/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/companies/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,Address,Phone,CompanyURL")] CompanyDetails companyDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companyDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyDetails);
        }

        // GET: admin/companies/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDetails = await _context.Companies.FindAsync(id);
            if (companyDetails == null)
            {
                return NotFound();
            }
            return View(companyDetails);
        }

        // POST: admin/companies/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,CompanyName,Address,Phone,CompanyURL")] CompanyDetails companyDetails)
        {
            if (id != companyDetails.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyDetailsExists(companyDetails.CompanyId))
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
            return View(companyDetails);
        }

        // GET: admin/companies/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDetails = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (companyDetails == null)
            {
                return NotFound();
            }

            return View(companyDetails);
        }

        // POST: admin/companies/delete/5
        // POST: admin/companies/delete/5
        [HttpPost("delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyDetails = await _context.Companies.FindAsync(id);
            if (companyDetails == null)
            {
                return NotFound();
            }

            try
            {
                _context.Companies.Remove(companyDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Check if the exception is due to a foreign key constraint violation
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    ModelState.AddModelError("", "Unable to delete. This company is referenced by existing policies.");
                    return View("Delete", companyDetails); // Return to the Delete view with an error message
                }

                // If it's a different issue, rethrow the exception
                throw;
            }
        }


        private bool CompanyDetailsExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }

    public class CompanyIndexViewModel
    {
        public IEnumerable<CompanyDetails> Companies { get; set; }
        public string SearchString { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
