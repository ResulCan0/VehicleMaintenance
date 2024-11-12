using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
[Authorize]
public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }
  
    // GET: Users
    public async Task<IActionResult> Index()
    {
        var users = await _context.CompanyUsers
            .Include(u => u.Company)
            .Include(u => u.Roles)// Şirket bilgilerini dahil et
            .ToListAsync();
        return View(users);
    }

    // GET: Users/Details/5
    public IActionResult Details(Guid id)
    {
        var users = _context.CompanyUsers.FirstOrDefault(c => c.UserId == id);
        if (users == null)
        {
            return NotFound();
        }
        return View(users);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        ViewData["CompanyId"] = new SelectList(_context.Companies.Where(c => !c.IsDeleted), "CompanyId", "CompanyName");
        ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RoleId,FirstName,LastName,Email,PhoneNumber,Password,IsActive,CompanyId")] User user)
    {
        if (ModelState.IsValid)
        {
            // Şirket ID'sinin geçerli olup olmadığını kontrol ediyoruz.
            if (!_context.Companies.Any(c => c.CompanyId == user.CompanyId))
            {
                ModelState.AddModelError("CompanyId", "Seçilen şirket geçersiz.");
                ViewData["CompanyId"] = new SelectList(_context.Companies.Where(c => !c.IsDeleted), "CompanyId", "CompanyName", user.CompanyId);
                ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
                return View(user);
            }

            // Aynı e-posta adresine sahip bir kullanıcı olup olmadığını kontrol ediyoruz.
            var existingUser = await _context.CompanyUsers.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi zaten kayıtlı.");
                ViewData["CompanyId"] = new SelectList(_context.Companies.Where(c => !c.IsDeleted), "CompanyId", "CompanyName", user.CompanyId);
                ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
                return View(user);
            }

            user.UserId = Guid.NewGuid();
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CompanyId"] = new SelectList(_context.Companies.Where(c => !c.IsDeleted), "CompanyId", "CompanyName", user.CompanyId);
        ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
        return View(user);
    }


    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var user = await _context.CompanyUsers.FindAsync(id);
        if (user == null) return NotFound();

        ViewData["CompanyId"] = new SelectList(_context.Companies.Where(c => !c.IsDeleted), "CompanyId", "CompanyName", user.CompanyId);
        ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
        return View(user);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("RoleId,UserId,FirstName,LastName,Email,PhoneNumber,Password,IsActive,CompanyId")] User user)
    {
        if (id != user.UserId)
            return NotFound();

        if (!CompanyExists(user.CompanyId))
        {
            ModelState.AddModelError("CompanyId", "Seçilen şirket geçerli değil.");
            ViewData["CompanyId"] = new SelectList(_context.Companies.Where(c => !c.IsDeleted), "CompanyId", "CompanyName", user.CompanyId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View(user);
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewData["CompanyId"] = new SelectList(_context.Companies.Where(c => !c.IsDeleted), "CompanyId", "CompanyName", user.CompanyId);
        ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
        return View(user);
    }

    private bool CompanyExists(Guid companyId)
    {
        return _context.Companies.Any(c => c.CompanyId == companyId);
    }


    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var user = await _context.CompanyUsers
            .Include(u => u.Company)
            .FirstOrDefaultAsync(m => m.UserId == id);
        if (user == null) return NotFound();

        return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var user = await _context.CompanyUsers.FindAsync(id);
        _context.CompanyUsers.Remove(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(Guid id)
    {
        return _context.CompanyUsers.Any(e => e.UserId == id);
    }
}
