using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VehicleMaintenance.Controllers
{
    public class FixedDefinitionController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FixedDefinitionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
            var fixeddefinitions = await _context.FixedDefinitions.ToListAsync();
            ViewBag.Fi = fixeddefinitions.Select(b => new { b.GroupCode, b.Code,b.Name }).ToList();
            return View(fixeddefinitions);
        }
        // POST: FixedDefinition/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: FixedDefinition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FixedDefinitionId,GroupCode,Code,Name")] FixedDefinition fixedDefinition)
        {

            if (ModelState.IsValid)
            {
                _context.Add(fixedDefinition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fixedDefinition);
        }

        // GET: FixedDefinition/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
           ViewData["FixedDefinitionId"] = new SelectList(
          "FixedDefinitionId",
          "GroupCode",
          "Code",
          "Name"
           );

            if (id == null)
            {
                return NotFound();
            }

            var FixedDefinition = await _context.FixedDefinitions.FindAsync(id);
            if (FixedDefinition == null)
            {
                return NotFound();
            }
            return View(FixedDefinition);
        }

        // GET: FixedDefinition/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FixedDefinitionId,GroupCode,Code,Name")] FixedDefinition fixedDefinition)
        {
            if (id != fixedDefinition.FixedDefinitionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fixedDefinition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixedDefinitionExists(fixedDefinition.FixedDefinitionId))
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
            return View(fixedDefinition);
        }
        // POST: FixedDefinition/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixedDefinition = await _context.FixedDefinitions
                .FirstOrDefaultAsync(m => m.FixedDefinitionId == id);
            if (fixedDefinition == null)
            {
                return NotFound();
            }

            return View(fixedDefinition);
        }

        // POST: FixedDefinition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fixeddefinition = await _context.FixedDefinitions.FindAsync(id);
            _context.FixedDefinitions.Remove(fixeddefinition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool FixedDefinitionExists(Guid id)
        {
            return _context.FixedDefinitions.Any(e => e.FixedDefinitionId == id);
        }
    }
}
