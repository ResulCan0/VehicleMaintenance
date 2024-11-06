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
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware'leri sýrasýyla ekleme
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Oturum middleware'ýný buraya taþýdýk
app.UseSession();
app.UseMiddleware<RequestLoggingMiddleware>();
// Kullanýcý giriþ kontrolü middleware'ý
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    // Giriþ yapýlmamýþsa ve giriþ sayfasý deðilse
    if (string.IsNullOrEmpty(context.Session.GetString("UserId")) && path != "/Account/Login" && context.Request.Method != "POST")
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next(); // Diðer middleware'larý çaðýr
});

// Kimlik doðrulama middleware'ýný ekle
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
