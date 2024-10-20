using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; // YourDbContext sýnýfýný barýndýran namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aþým süresi
    options.Cookie.HttpOnly = true; // Çerez yalnýzca HTTP üzerinden eriþilebilir
    options.Cookie.IsEssential = true; // Çerez, kullanýcý rýzasý olmadan ayarlanabilir
});

// Veritabaný baðlantýsýný yapýlandýrma
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Connection string'i ayarlayýn

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Oturum middleware'ýný buraya taþýdýk
app.UseSession();

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


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
