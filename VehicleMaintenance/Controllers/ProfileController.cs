using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Session için ekledik
using System;
using System.Threading.Tasks;
using VehicleMaintenance.Models; // Model namespace'inizi belirtin

// Sadece giriş yapmış kullanıcıların erişimine izin ver
public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Profil sayfası için GET metodu
    public async Task<IActionResult> Index()
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = Guid.Parse(userIdString);
        var user = await _context.CompanyUsers.FindAsync(userId);
        if (user == null)
        {
            return NotFound(); // Kullanıcı bulunamazsa 404 döner
        }

        return View(user); // Profil bilgilerini içeren view'i döner
    }

    // Profil güncelleme sayfası için GET metodu
    [HttpGet]
    public async Task<IActionResult> Update()
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = Guid.Parse(userIdString);
        var user = await _context.CompanyUsers.FindAsync(userId);
        if (user == null)
        {
            return NotFound(); // Kullanıcı bulunamazsa 404 döner
        }

        return View(user); // Kullanıcı bilgilerini düzenleme için döner
    }

    // Profil güncelleme için POST metodu
    [HttpPost]
    public async Task<IActionResult> Update(User user)
    {
        if (ModelState.IsValid)
        {
            // Mevcut kullanıcıyı veritabanından bul
            var existingUser = await _context.CompanyUsers.FindAsync(user.UserId);
            if (existingUser == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döner
            }

            // Güncellenmiş bilgileri mevcut kullanıcıya aktar
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            // Eğer şifre güncellenmesini istiyorsanız buraya ekleyebilirsiniz.

            _context.Update(existingUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index"); // Güncellemeden sonra profil sayfasına döner
        }

        return View(user); // Hata varsa mevcut bilgileri döner
    }
}
