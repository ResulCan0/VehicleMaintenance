using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ActiveModulesViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ActiveModulesViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (Guid.TryParse(userIdString, out Guid userId))
        {
            var user = await _context.CompanyUsers.FindAsync(userId);
            if (user != null)
            {
                var activeModules = await _context.CompanyModules
                    .Where(cm => cm.CompanyId == user.CompanyId && cm.IsActive)
                    .Select(cm => cm.Module)
                    .ToListAsync();

                return View(activeModules);
            }
        }
        return View(new List<Module>()); // Boş bir liste döndür
    }
}
