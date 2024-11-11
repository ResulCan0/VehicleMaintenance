using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                var user = _context.CompanyUsers
                    .Include(u => u.Roles)
                    .SingleOrDefault(u => u.Email == model.Email && u.IsActive);

                if (user != null)
                {
                    if (user.Password == model.Password) // Burada hash karşılaştırması yapmalısınız
                    {
                        // Claims listesi oluştur
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim("CompanyId", user.CompanyId.ToString()),
                            new Claim(ClaimTypes.Role, user.Roles.RoleName)
                        };

                        // Kullanıcının bağlı olduğu aktif modülleri de claim olarak ekleyin
                        var activeModules = _context.CompanyModules
                            .Where(cm => cm.CompanyId == user.CompanyId && cm.IsActive)
                            .Select(cm => cm.Module)
                            .ToList();

                        foreach (var module in activeModules)
                        {
                            claims.Add(new Claim("Module", module.ModuleId.ToString()));
                        }

                        // ClaimsIdentity ve ClaimsPrincipal oluştur
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        // Oturum aç
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                        // Yönlendirme
                        return RedirectToAction("Index", "StockPart");
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

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // Aktif modül erişimi kontrolü için bir yardımcı metot ekleyin
        [Authorize]
        public bool HasAccessToModule(Guid moduleId)
        {
            var userClaims = HttpContext.User.Claims.Where(c => c.Type == "Module").Select(c => Guid.Parse(c.Value)).ToList();
            return userClaims.Contains(moduleId);
        }
    }
}
