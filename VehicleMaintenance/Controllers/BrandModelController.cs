using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace VehicleMaintenance.Controllers
{
    public class BrandModelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandModelController(ApplicationDbContext context)
        {
            _context = context;
        }
   
        // GET: BrandModel
        public async Task<IActionResult> Index()
        {
        
            var brandModels = _context.BrandModels.Include(b => b.Brand);
            return View(await brandModels.ToListAsync());
        }

        // GET: BrandModel/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            return View();
        }

        // POST: BrandModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandModelId,ModelName,Class,BrandId")] BrandModel brandModel)
        {
            if (ModelState.IsValid)
            {
                brandModel.BrandModelId = Guid.NewGuid();
                _context.Add(brandModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", brandModel.BrandId);
            return View(brandModel);
        }

        // GET: BrandModel/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brandModel = await _context.BrandModels.FindAsync(id);
            if (brandModel == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", brandModel.BrandId);
            return View(brandModel);
        }

        // POST: BrandModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BrandModelId,ModelName,Class,BrandId")] BrandModel brandModel)
        {
            if (id != brandModel.BrandModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brandModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandModelExists(brandModel.BrandModelId))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", brandModel.BrandId);
            return View(brandModel);
        }

        // GET: BrandModel/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brandModel = await _context.BrandModels
                .Include(b => b.Brand)
                .FirstOrDefaultAsync(m => m.BrandModelId == id);
            if (brandModel == null)
            {
                return NotFound();
            }

            return View(brandModel);
        }

        // POST: BrandModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var brandModel = await _context.BrandModels.FindAsync(id);
            _context.BrandModels.Remove(brandModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandModelExists(Guid id)
        {
            return _context.BrandModels.Any(e => e.BrandModelId == id);
        }
    }
}
