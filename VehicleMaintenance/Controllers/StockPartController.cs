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
    public async Task<IActionResult> Edit(StockPart stockPart)
    {
        if (ModelState.IsValid)
        {
            _context.Update(stockPart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(stockPart);
    }

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
