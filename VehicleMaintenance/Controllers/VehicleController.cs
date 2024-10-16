using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicle
        public async Task<IActionResult> Index()
        {
            
            var vehicles = _context.Vehicles.Include(v => v.BrandModel.Brand).Include(v => v.Company);
            return View(await vehicles.ToListAsync());
        }

        // GET: Vehicle/Create
        public IActionResult Create()
        {

            
            ViewData["BrandModelId"] = new SelectList(
            _context.BrandModels.Include(b => b.Brand), // Brand ile dahil et
            "BrandModelId",
            "BrandModelText"
             );

            //ViewData["BrandModelId"] = new SelectList(_context.BrandModels, "BrandModel.BrandId", "BrandName");//hatalı şuanda aktif değil
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View();
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,PlateNumber,VehicleType,Year,BrandModelId,CompanyId")] Vehicle vehicle)
        {
            var brandModelExists = _context.BrandModels.Any(bm => bm.BrandModelId == vehicle.BrandModelId);
            if (!brandModelExists)
            {
                if (vehicle.BrandModelId == Guid.Empty)
                {
                    ModelState.AddModelError("BrandModelId", "Brand Model is required.");
                    return View(vehicle);
                }

                ModelState.AddModelError("BrandModelId", "Invalid Brand Model.");
                return View(vehicle); // Veya uygun bir hata mesajı döndürebilirsiniz
            }

            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            ViewData["BrandModelId"] = new SelectList(
          _context.BrandModels.Include(b => b.Brand), // Brand ile dahil et
          "BrandModelId",
          "BrandModelText"
           );

            //ViewData["BrandModelId"] = new SelectList(_context.BrandModels, "BrandModel.BrandId", "BrandName");//hatalı şuanda aktif değil
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VehicleId,PlateNumber,VehicleType,Year,BrandModelId,CompanyId")] Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.VehicleId))
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
            return View(vehicle);
        }

        // GET: Vehicle/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.BrandModel.Brand)
                .Include(v => v.Company)
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(Guid id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }
}
