using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LeavesTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeavesTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeavesTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.LeavesType.ToListAsync());
        }

        // GET: LeavesTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leavesType = await _context.LeavesType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leavesType == null)
            {
                return NotFound();
            }

            return View(leavesType);
        }

        // GET: LeavesTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeavesTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,CreatedById,CreatedON,ModifiedById,ModifiedON")] LeavesType leavesType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leavesType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leavesType);
        }

        // GET: LeavesTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leavesType = await _context.LeavesType.FindAsync(id);
            if (leavesType == null)
            {
                return NotFound();
            }
            return View(leavesType);
        }

        // POST: LeavesTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,CreatedById,CreatedON,ModifiedById,ModifiedON")] LeavesType leavesType)
        {
            if (id != leavesType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leavesType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeavesTypeExists(leavesType.Id))
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
            return View(leavesType);
        }

        // GET: LeavesTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leavesType = await _context.LeavesType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leavesType == null)
            {
                return NotFound();
            }

            return View(leavesType);
        }

        // POST: LeavesTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leavesType = await _context.LeavesType.FindAsync(id);
            if (leavesType != null)
            {
                _context.LeavesType.Remove(leavesType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeavesTypeExists(int id)
        {
            return _context.LeavesType.Any(e => e.Id == id);
        }
    }
}
