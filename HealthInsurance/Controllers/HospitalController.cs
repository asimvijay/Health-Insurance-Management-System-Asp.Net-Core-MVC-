using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Entities;

[Route("admin/[controller]")]
public class HospitalController : Controller
{
    private readonly AppDbContext _context;
    private const int PageSize = 10; // Number of hospitals per page

    public HospitalController(AppDbContext context)
    {
        _context = context;
    }

    // GET: admin/hospital/index
    [HttpGet("")]
    public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
    {
        var hospitals = from h in _context.Hospitals
                        select h;

        if (!string.IsNullOrEmpty(searchString))
        {
            hospitals = hospitals.Where(h => h.HospitalName.Contains(searchString));
        }

        var totalItems = await hospitals.CountAsync();
        var hospitalsToDisplay = await hospitals
            .OrderBy(h => h.HospitalId)
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var viewModel = new HospitalIndexViewModel
        {
            Hospitals = hospitalsToDisplay,
            SearchString = searchString,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize)
        };

        return View(viewModel);
    }

    // GET: admin/hospital/details/5
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hospitalInfo = await _context.Hospitals
            .FirstOrDefaultAsync(m => m.HospitalId == id);
        if (hospitalInfo == null)
        {
            return NotFound();
        }

        return View(hospitalInfo);
    }

    // GET: admin/hospital/create
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: admin/hospital/create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("HospitalId,HospitalName,PhoneNo,Location,URL")] HospitalInfo hospitalInfo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(hospitalInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(hospitalInfo);
    }

    // GET: admin/hospital/edit/5
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hospitalInfo = await _context.Hospitals.FindAsync(id);
        if (hospitalInfo == null)
        {
            return NotFound();
        }
        return View(hospitalInfo);
    }

    // POST: admin/hospital/edit/5
    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("HospitalId,HospitalName,PhoneNo,Location,URL")] HospitalInfo hospitalInfo)
    {
        if (id != hospitalInfo.HospitalId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hospitalInfo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HospitalInfoExists(hospitalInfo.HospitalId))
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
        return View(hospitalInfo);
    }

    // GET: admin/hospital/delete/5
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hospitalInfo = await _context.Hospitals
            .FirstOrDefaultAsync(m => m.HospitalId == id);
        if (hospitalInfo == null)
        {
            return NotFound();
        }

        return View(hospitalInfo);
    }

    // POST: admin/hospital/delete/5
    [HttpPost("delete/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hospitalInfo = await _context.Hospitals.FindAsync(id);
        if (hospitalInfo != null)
        {
            _context.Hospitals.Remove(hospitalInfo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HospitalInfoExists(int id)
    {
        return _context.Hospitals.Any(e => e.HospitalId == id);
    }
}
