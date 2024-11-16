using Microsoft.AspNetCore.Mvc;
using System;

namespace VehicleMaintenance.Controllers
{
    public class LocalizeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocalizeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listele
        public IActionResult Index()
        {
            var featuredNumbers = _context.FeaturedNumbers.ToList();
            return View(featuredNumbers);
        }

        // Düzenle
        public IActionResult Edit(int id)
        {
            var featuredNumber = _context.FeaturedNumbers.Find(id);
            if (featuredNumber == null)
            {
                return NotFound();
            }
            return View(featuredNumber);
        }

        [HttpPost]
        public IActionResult Edit(FeaturedNumber model)
        {
            var entity = _context.FeaturedNumbers.Find(model.Id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Value = model.Value;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
