using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Entities;

namespace HealthInsurance.Controllers
{
    public class PoliciesOnEmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public PoliciesOnEmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PoliciesOnEmployees
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PoliciesOnEmployees.Include(p => p.Employee).Include(p => p.Policy);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PoliciesOnEmployees/Details/5
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

        // GET: PoliciesOnEmployees/Create
        public IActionResult Create()
        {
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "FirstName");
            ViewData["PolicyId"] = new SelectList(_context.Policies, "PolicyId", "PolicyName");
            return View();
        }

        // POST: PoliciesOnEmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmpNo,PolicyId,StartDate,EndDate,EMI")] PoliciesOnEmployees policiesOnEmployees)
        {
            if (ModelState.IsValid)
            {
                _context.Add(policiesOnEmployees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "FirstName", policiesOnEmployees.EmpNo);
            ViewData["PolicyId"] = new SelectList(_context.Policies, "PolicyId", "PolicyName", policiesOnEmployees.PolicyId);
            return View(policiesOnEmployees);
        }

        // GET: PoliciesOnEmployees/Edit/5
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

        // POST: PoliciesOnEmployees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmpNo,PolicyId,StartDate,EndDate,EMI")] PoliciesOnEmployees policiesOnEmployees)
        {
            if (id != policiesOnEmployees.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policiesOnEmployees);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "FirstName", policiesOnEmployees.EmpNo);
            ViewData["PolicyId"] = new SelectList(_context.Policies, "PolicyId", "PolicyName", policiesOnEmployees.PolicyId);
            return View(policiesOnEmployees);
        }

        // GET: PoliciesOnEmployees/Delete/5
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

        // POST: PoliciesOnEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policiesOnEmployees = await _context.PoliciesOnEmployees.FindAsync(id);
            if (policiesOnEmployees != null)
            {
                _context.PoliciesOnEmployees.Remove(policiesOnEmployees);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PoliciesOnEmployeesExists(int id)
        {
            return _context.PoliciesOnEmployees.Any(e => e.Id == id);
        }
    }
}
