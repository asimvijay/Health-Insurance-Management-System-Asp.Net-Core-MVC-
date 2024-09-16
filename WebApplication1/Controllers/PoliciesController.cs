using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // <-- Add this line
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Entities;
using HealthInsurance.Models;

namespace HealthInsurance.Controllers
{
    [Route("admin/[controller]")]
    public class PoliciesController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10; // Number of policies per page

        public PoliciesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/policies/index
        [HttpGet("")]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            var policies = from p in _context.Policies.Include(p => p.Company)
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                policies = policies.Where(p => p.PolicyName.Contains(searchString));
            }

            var totalItems = await policies.CountAsync();
            var policiesToDisplay = await policies
                .OrderBy(p => p.PolicyId)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var viewModel = new PolicyIndexViewModel
            {
                Policies = policiesToDisplay,
                SearchString = searchString,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize)
            };

            return View(viewModel);
        }

        // GET: admin/policies/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.PolicyId == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // GET: admin/policies/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View();
        }

        // POST: admin/policies/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PolicyId,PolicyName,PolicyDesc,PolicyAmount,EMI,CompanyId,Medicaid")] PolicyDto policyDto)
        {
            if (ModelState.IsValid)
            {
                // Convert PolicyDto to Policy entity
                var policy = new Policy
                {
                    PolicyId = policyDto.PolicyId,
                    PolicyName = policyDto.PolicyName,
                    PolicyDesc = policyDto.PolicyDesc,
                    PolicyAmount = policyDto.PolicyAmount,
                    EMI = policyDto.EMI,
                    CompanyId = policyDto.CompanyId,
                    Medicaid = policyDto.Medicaid
                };

                _context.Policies.Add(policy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, repopulate the dropdown
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", policyDto.CompanyId);
            return View(policyDto);
        }


        // GET: admin/policies/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", policy.CompanyId);
            return View(policy);
        }

        // POST: admin/policies/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PolicyId,PolicyName,PolicyDesc,PolicyAmount,EMI,CompanyId,Medicaid")] Policy policy)
        {
            if (id != policy.PolicyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyExists(policy.PolicyId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", policy.CompanyId);
            return View(policy);
        }

        // GET: admin/policies/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.PolicyId == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // POST: admin/policies/delete/5
        [HttpPost("delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PolicyExists(int id)
        {
            return _context.Policies.Any(e => e.PolicyId == id);
        }
    }

    public class PolicyIndexViewModel
    {
        public IEnumerable<Policy> Policies { get; set; }
        public string SearchString { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
