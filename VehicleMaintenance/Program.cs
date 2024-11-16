using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // Cookie kimlik doðrulama için ekledik

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Session ayarlarý
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aþým süresi
    options.Cookie.HttpOnly = true; // Çerez yalnýzca HTTP üzerinden eriþilebilir
    options.Cookie.IsEssential = true; // Çerez, kullanýcý rýzasý olmadan ayarlanabilir
    options.Cookie.SameSite = SameSiteMode.Lax; // Çapraz site istekleri için
});

// Veritabaný baðlantýsýný yapýlandýrma
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(EncryptionService.Decrypt(builder.Configuration.GetConnectionString("DefaultConnection")))); // Connection string'i ayarlayýn

// Kimlik doðrulama ayarlarý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giriþ yolu
        options.LogoutPath = "/Account/Logout"; // Çýkýþ yolu
        options.SlidingExpiration = true; // Oturum yenilemesi (çerez süresi dolsa bile)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Çerezin geçerlilik süresi
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30); // Çerezlerin ömrü
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Çerezin geçerlilik süresi
        options.Events.OnSigningOut = async context =>
        {
            // Kullanýcý çýkýþ yaptýðýnda cookie'leri temizleme
            context.Response.Cookies.Delete("AspNetCore.Cookies"); // Çerez adýný doðru girin
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
DynamicContentHelper.Configure(app.Services);
// Middleware'leri sýrasýyla ekleme
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Oturum middleware'ýný buraya taþýdýk
app.UseSession();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<VisitorTrackingMiddleware>();

// Kimlik doðrulama middleware'ýný ekle
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
