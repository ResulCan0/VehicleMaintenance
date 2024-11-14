using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

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
    }
}
