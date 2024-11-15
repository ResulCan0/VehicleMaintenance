using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VehicleMaintenance.Models;

public class VisitorTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<VisitorTrackingMiddleware> _logger;

    public VisitorTrackingMiddleware(RequestDelegate next, ILogger<VisitorTrackingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
    {
        try
        {
            // Oturum kontrolü
            if (!context.Session.Keys.Contains("HasVisited"))
            {
                var siteVisitCount = await dbContext.SiteStatistics.FirstOrDefaultAsync();

                if (siteVisitCount == null)
                {
                    siteVisitCount = new SiteStatistic { VisitCount = 1, LastVisitDate = DateTime.UtcNow };
                    dbContext.SiteStatistics.Add(siteVisitCount);
                }
                else
                {
                    siteVisitCount.VisitCount++;
                    siteVisitCount.LastVisitDate = DateTime.UtcNow;
                    dbContext.SiteStatistics.Update(siteVisitCount);
                }

                await dbContext.SaveChangesAsync();

                // Oturuma bir anahtar ekle
                context.Session.SetString("HasVisited", "true");

                _logger.LogInformation($"Ziyaretçi sayısı güncellendi: {siteVisitCount.VisitCount}");
            }
            else
            {
                _logger.LogInformation("Ziyaretçi zaten oturumda mevcut.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ziyaretçi sayısını kaydederken bir hata oluştu.");
        }

        // Bir sonraki middleware'e geç
        await _next(context);
    }
}
