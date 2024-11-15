using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Site istatistiklerini asenkron olarak getir
        var siteStatistic = await _context.SiteStatistics.FirstOrDefaultAsync();
        ViewBag.VisitorCount = siteStatistic?.VisitCount ?? 0;

        // Başvuruları asenkron olarak getir
        var applications = await _context.Applications.ToListAsync();

        // ViewModel'i doldur
        var model = new CRMViewModel
        {
            Applications = applications
        };

        return View(model);
    }
}
