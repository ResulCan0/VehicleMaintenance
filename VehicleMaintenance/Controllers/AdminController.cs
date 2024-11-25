using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Edit(Guid id)
    {
        var application = await _context.Applications.FindAsync(id);
        if (application == null)
        {
            return NotFound(); // NotFound() will return a 404 response
        }

        // Here, you can return the application data to the Edit view
        return View(application);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Application model)
    {
   
            var application = await _context.Applications.FindAsync(model.Id);
            if (application == null)
            {
                return NotFound(); // If the application is not found, return a 404 response
            }

            // Update the application details
            application.SalesStatus = model.SalesStatus;
            application.Reason = model.Reason;

            // Asynchronously save the changes
            await _context.SaveChangesAsync();

            // Redirect to the Index action
            return RedirectToAction("Index");
       

        // If model is not valid, return the same view with validation errors
       
    }
}
