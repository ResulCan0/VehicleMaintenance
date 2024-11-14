using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize]
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
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = userIdClaim?.Value;
        var userIdClaimRole = User.FindFirst(ClaimTypes.Role);
        var userIdRole = userIdClaimRole?.Value;


        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _context.CompanyUsers
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

        if (user == null)
        {
            return NotFound();
        }

        // Check if the user is an admin
        var isAdmin = userIdRole;

        if (isAdmin =="ADMİN")
        {
            var companiesr=await _context.Companies.Where(c => !c.IsDeleted).ToListAsync();
            return View(companiesr);
        }
        else
        {
            var companiesrr = await _context.Companies.Where(c => c.CompanyId == user.CompanyId && !c.IsDeleted).ToListAsync();
            return View(companiesrr);
        }
        // If the user is an admin, fetch all companies, else fetch only their own company
       
    }

    // GET: Company/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = userIdClaim?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Ok(null);
        }
        return Ok(userId);
    }

    // GET: Company/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = userIdClaim?.Value;
        var userIdClaimRole = User.FindFirst(ClaimTypes.Role);
        var userIdRole = userIdClaimRole?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _context.CompanyUsers
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

        if (user == null)
        {
            return NotFound();
        }

        // Check if the user is an admin or if they belong to the company
        var company = await _context.Companies
            .Where(c => c.CompanyId == id && (c.CompanyId == user.CompanyId || userIdRole == "ADMİN"))
            .FirstOrDefaultAsync();

        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // POST: Company/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,IsActive,TaxNumber,Adress,Mail,TaxOffice,PersonandLegal,WorkingType,MaturityDate,PhoneNumber")] Company company)
    {
        if (ModelState.IsValid)
        {
            var existingCompany = await _context.Companies.FirstOrDefaultAsync(u => u.CompanyName == company.CompanyName);
            if (existingCompany != null)
            {
                ModelState.AddModelError("CompanyName", "Bu şirket zaten kayıtlı.");
                return View(company);
            }

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

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = userIdClaim?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _context.CompanyUsers
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

        if (user == null)
        {
            return NotFound();
        }
        var userIdClaimRole = User.FindFirst(ClaimTypes.Role);
        var userIdRole = userIdClaimRole?.Value;
        // Ensure the user can only edit their own company or if they are an admin
        var company = await _context.Companies
            .Where(c => c.CompanyId == id && (c.CompanyId == user.CompanyId || userIdRole =="ADMİN"))
            .FirstOrDefaultAsync();

        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // POST: Company/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("CompanyId,CompanyName,IsActive,TaxNumber,Adress,Mail,TaxOffice,PersonandLegal,WorkingType,MaturityDate,PhoneNumber")] Company company)
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
                throw;
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

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = userIdClaim?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _context.CompanyUsers
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

        if (user == null)
        {
            return NotFound();
        }
        var userIdClaimRole = User.FindFirst(ClaimTypes.Role);
        var userIdRole = userIdClaimRole?.Value;
        // Ensure the user can only delete their own company or if they are an admin
        var company = await _context.Companies
            .Where(c => c.CompanyId == id && (c.CompanyId == user.CompanyId || userIdRole=="ADMİN"))
            .FirstOrDefaultAsync();

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
            company.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CompanyExists(Guid id)
    {
        return _context.Companies.Any(e => e.CompanyId == id);
    }
}
