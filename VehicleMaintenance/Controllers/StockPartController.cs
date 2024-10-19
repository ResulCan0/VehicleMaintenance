using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class StockPartController : Controller
{
    private readonly ApplicationDbContext _context;

    public StockPartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.StockParts.ToListAsync());
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(StockPart stockPart)
    {
        if (ModelState.IsValid)
        {
            stockPart.PartId = Guid.NewGuid(); // Generate new GUID for the PartId
            _context.StockParts.Add(stockPart);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // If the model state is not valid, redisplay the form
        return View(stockPart);
    }


    public async Task<IActionResult> Details(Guid id)
    {
        var stockPart = await _context.StockParts.FindAsync(id);
        if (stockPart == null)
        {
            return NotFound();
        }
        return View(stockPart);
    }

    public IActionResult Edit(Guid id)
    {
        var stockPart = _context.StockParts.Find(id);
        if (stockPart == null)
        {
            return NotFound();
        }
        return View(stockPart);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("PartId,PartNumber,PartName,PartDate,PartAmount,StockQuantity")] StockPart stockPart)
    {
        if (id != stockPart.PartId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
               
                _context.Update(stockPart);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.StockParts.Any(e => e.PartId == id))
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
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var stockPart = await _context.StockParts.FindAsync(id);
        if (stockPart == null)
        {
            return NotFound();
        }
        return View(stockPart);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var stockPart = await _context.StockParts.FindAsync(id);
        _context.StockParts.Remove(stockPart);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
