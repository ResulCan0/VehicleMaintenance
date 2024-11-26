using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VehicleMaintenance.Models;

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

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var application = _context.Applications.Find(id);
        if (application == null)
            return NotFound();

        return PartialView("Edit", application); // Partial view olarak render et
    }

    // Edit işlemi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Application model)
    {
        
            

        var application = _context.Applications.Find(model.Id);
        if (application == null)
            return NotFound();

        application.SalesStatus = model.SalesStatus;
        application.Reason = model.Reason;

        _context.SaveChanges();
        return Ok(); // 200 OK döndür
    }
}
