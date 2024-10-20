using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class CompanyController : Controller
{
    private readonly ApplicationDbContext _context;

    public CompanyController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Company
    public async Task<IActionResult> Index()
    {
        var companies = await _context.Companies
        .Where(c => !c.IsDeleted) // Silinmemiş kayıtları al
        .ToListAsync();

        return View(companies);
    }

    // GET: Company/Create
    public IActionResult Create()
    {
        return View();
    }
    // GET: Company/Details/5
    public IActionResult Details(Guid id)
    {
        var company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
        if (company == null)
        {
            return NotFound();
        }
        return View(company);
    }
    // POST: Company/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,IsActive,TaxNumber")] Company company)
    {
        if (ModelState.IsValid)
        {
            _context.Add(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(company);
    }

    // GET: Company/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = await _context.Companies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }
        return View(company);
    }

    // POST: Company/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("CompanyId,CompanyName,IsActive,TaxNumber")] Company company)
    {
        if (id != company.CompanyId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(company);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(company.CompanyId))
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
        return View(company);
    }

    // GET: Company/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = await _context.Companies
            .FirstOrDefaultAsync(m => m.CompanyId == id);
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // POST: Company/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
    var company = await _context.Companies.FindAsync(id);
    if (company != null)
    {
        company.IsDeleted = true; // Kaydı silmek yerine işaretle
        await _context.SaveChangesAsync();
    }
    return RedirectToAction(nameof(Index));
    }

    private bool CompanyExists(Guid id)
    {
        return _context.Companies.Any(e => e.CompanyId == id);
    }
}
