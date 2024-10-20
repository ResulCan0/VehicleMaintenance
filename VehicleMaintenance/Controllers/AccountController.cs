using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using VehicleMaintenance.Models;

namespace VehicleMaintenance.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.CompanyUsers.SingleOrDefault(u => u.Email == model.Email && u.IsActive);
                if (user != null)
                {
                    if (user.Password == model.Password) // Burada hash karşılaştırması yapmanız gerekir
                    {
                        // Kullanıcıyı oturum açtır
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                        HttpContext.Session.SetString("CompanyId", user.CompanyId.ToString());

                        // Kullanıcının bağlı olduğu şirketin aktif modüllerini al
                        var activeModules = _context.CompanyModules
                            .Where(cm => cm.CompanyId == user.CompanyId && cm.IsActive)
                            .Select(cm => cm.Module)
                            .ToList();

                        // Modül erişimini session'a kaydet
                        HttpContext.Session.SetString("ActiveModules", string.Join(",", activeModules.Select(m => m.ModuleId)));
                        ViewBag.ActiveModules = activeModules;
                        return RedirectToAction("Index", "StockPart"); // Ana sayfaya yönlendir
                    }
                    ModelState.AddModelError("", "Geçersiz şifre.");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı veya aktif değil.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Oturumu sonlandır
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        // Aktif modülleri kontrol etmek için bir yardımcı metot ekleyebilirsiniz
        public bool HasAccessToModule(Guid moduleId)
        {
            var activeModules = HttpContext.Session.GetString("ActiveModules")?.Split(',')
                .Select(Guid.Parse).ToList() ?? new List<Guid>();

            return activeModules.Contains(moduleId);
        }
    }
}
