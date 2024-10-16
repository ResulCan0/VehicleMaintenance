using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Controllers
{
    public class VehicleMaintenancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleMaintenancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VehicleMaintenances
        public async Task<IActionResult> Index()
        {
            var vehicleMaintenances = _context.VehicleMaintenances.Include(vm => vm.Vehicle);
            return View(await vehicleMaintenances.ToListAsync());
        }

        // GET: VehicleMaintenances/Create
        public IActionResult Create()
        {
            // Create a SelectList for vehicles displaying BrandModelText and returning VehicleId
            ViewBag.VehicleId = new SelectList(_context.Vehicles.Include(v => v.BrandModel).ThenInclude(bm => bm.Brand),
                                               "VehicleId",
                                               "BrandModel.BrandModelText");
            return View();
        }

        // POST: VehicleMaintenances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleMaintenanceId,MaintenanceType,ReasonCode,Description,ChargeParts,VehicleId")] VehicleMaintenances vehicleMaintenance)
        {
            // Create a SelectList for vehicles displaying BrandModelText and returning VehicleId
            ViewBag.VehicleId = new SelectList(_context.Vehicles.Include(v => v.BrandModel).ThenInclude(bm => bm.Brand),
                                               "VehicleId",
                                               "BrandModel.BrandModelText");
            if (ModelState.IsValid)
            {
                _context.Add(vehicleMaintenance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleMaintenance);
        }

        // GET: VehicleMaintenances/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            // Create a SelectList for vehicles displaying BrandModelText and returning VehicleId
            ViewBag.VehicleId = new SelectList(_context.Vehicles.Include(v => v.BrandModel).ThenInclude(bm => bm.Brand),
                                               "VehicleId",
                                               "BrandModel.BrandModelText");
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMaintenance = await _context.VehicleMaintenances.FindAsync(id);
            if (vehicleMaintenance == null)
            {
                return NotFound();
            }
            return View(vehicleMaintenance);
        }

        // POST: VehicleMaintenances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VehicleMaintenanceId,MaintenanceType,ReasonCode,Description,ChargeParts,VehicleId")] VehicleMaintenances vehicleMaintenance)
        {
            if (id != vehicleMaintenance.VehicleMaintenanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleMaintenance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleMaintenanceExists(vehicleMaintenance.VehicleMaintenanceId))
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
            return View(vehicleMaintenance);
        }

        // GET: VehicleMaintenances/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMaintenance = await _context.VehicleMaintenances
                .Include(vm => vm.Vehicle)
                .FirstOrDefaultAsync(m => m.VehicleMaintenanceId == id);
            if (vehicleMaintenance == null)
            {
                return NotFound();
            }

            return View(vehicleMaintenance);
        }

        // POST: VehicleMaintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vehicleMaintenance = await _context.VehicleMaintenances.FindAsync(id);
            _context.VehicleMaintenances.Remove(vehicleMaintenance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleMaintenanceExists(Guid id)
        {
            return _context.VehicleMaintenances.Any(e => e.VehicleMaintenanceId == id);
        }
    }
}
