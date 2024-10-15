using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Controllers
{
    public class StockPartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockPartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StockPart
        public async Task<IActionResult> Index()
        {
            return View(await _context.StockParts.ToListAsync());
        }

        // GET: StockPart/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StockPart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockPartId,PartName,StockQuantity")] StockPart stockPart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stockPart);
        }

        // GET: StockPart/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockPart = await _context.StockParts.FindAsync(id);
            if (stockPart == null)
            {
                return NotFound();
            }
            return View(stockPart);
        }

        // POST: StockPart/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("StockPartId,PartName,StockQuantity")] StockPart stockPart)
        {
            if (id != stockPart.PartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockPart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockPartExists(stockPart.PartId))
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
            return View(stockPart);
        }

        // GET: StockPart/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockPart = await _context.StockParts
                .FirstOrDefaultAsync(m => m.PartId == id);
            if (stockPart == null)
            {
                return NotFound();
            }

            return View(stockPart);
        }

        // POST: StockPart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var stockPart = await _context.StockParts.FindAsync(id);
            _context.StockParts.Remove(stockPart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockPartExists(Guid id)
        {
            return _context.StockParts.Any(e => e.PartId == id);
        }
    }
}
