using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;



    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
        var brands = await _context.Brands.ToListAsync();
        ViewBag.Brands = brands.Select(b => new { b.BrandName, b.Logo }).ToList();
        return View(brands);
        }

        // GET: Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BrandId,BrandName")] Brand brand, IFormFile LogoFile)
    {
        if (ModelState.IsValid)
        {
            if (LogoFile != null && LogoFile.ContentType == "image/svg+xml")
            {
                using (var reader = new StreamReader(LogoFile.OpenReadStream()))
                {
                    var svgContent = await reader.ReadToEndAsync();
                    brand.Logo = SetSvgDimensions(svgContent, 50, 50); // Boyutları ayarla
                }
            }

            _context.Add(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(brand);
    }


    // GET: Brand/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BrandId,BrandName")] Brand brand, IFormFile LogoFile)
    {
        if (id != brand.BrandId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {

            var existingBrand = await _context.Brands.FindAsync(id);
            if (existingBrand == null)
            {
                return NotFound();
            }

            existingBrand.BrandName = brand.BrandName;

            // SVG logo güncelleme
            if (LogoFile != null && LogoFile.ContentType == "image/svg+xml")
            {
                using (var reader = new StreamReader(LogoFile.OpenReadStream()))
                {
                    existingBrand.Logo = await reader.ReadToEndAsync();
                    existingBrand.Logo = SetSvgDimensions(existingBrand.Logo, 50, 50);
                }
            }

            _context.Update(existingBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(brand);
    }
         
        // GET: Brand/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var brand = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(Guid id)
        {
            return _context.Brands.Any(e => e.BrandId == id);
        }
    private string SetSvgDimensions(string svgContent, int width, int height)
    {
        if (string.IsNullOrEmpty(svgContent)) return svgContent;

        // width ve height özelliklerini değiştirmek için Regex kullanıyoruz
        svgContent = Regex.Replace(svgContent, @"width=""[^""]*""", $"width=\"{width}px\"");
        svgContent = Regex.Replace(svgContent, @"height=""[^""]*""", $"height=\"{height}px\"");

        // Eğer width ve height yoksa, SVG etiketine ekleyin
        if (!svgContent.Contains("width="))
            svgContent = svgContent.Replace("<svg", $"<svg width=\"{width}px\"");
        if (!svgContent.Contains("height="))
            svgContent = svgContent.Replace("<svg", $"<svg height=\"{height}px\"");

        return svgContent;
    }
}

