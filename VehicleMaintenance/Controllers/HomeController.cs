using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using VehicleMaintenance.Models;
using VehicleMaintenance.Views.Home;

namespace VehicleMaintenance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply(Application model)
        {
            if (ModelState.IsValid)
            {
                // ApplyModel'ini Application varlýðýna dönüþtür
                var application = new Application
                {
                    Name = model.Name,
                    Email = model.Email,
                    // Checkbox deðerlerini 0 veya 1'den true/false'a dönüþtür
                    VehicleTracking = model.VehicleTracking ,    // 0 veya 1'i true/false'a dönüþtür
                    StockTracking = model.StockTracking ,        // 0 veya 1'i true/false'a dönüþtür
                    Ecommerce = model.Ecommerce ,                // 0 veya 1'i true/false'a dönüþtür
                    SeoOptimization = model.SeoOptimization ,    // 0 veya 1'i true/false'a dönüþtür
                    Message = model.Message
                };

                // Veritabanýna kaydet
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                // Baþarý mesajý veya yönlendirme
                return Json(new { success = true, message = "Baþvurunuz baþarýyla kaydedildi!" });
            }

            // Model geçerli deðilse hata mesajý dön
            return Json(new { success = false, message = "Formda eksik alanlar var!" });
        }
    }
}
