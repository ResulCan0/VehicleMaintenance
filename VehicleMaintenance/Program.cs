using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; // YourDbContext s�n�f�n� bar�nd�ran namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman a��m s�resi
    options.Cookie.HttpOnly = true; // �erez yaln�zca HTTP �zerinden eri�ilebilir
    options.Cookie.IsEssential = true; // �erez, kullan�c� r�zas� olmadan ayarlanabilir
});

// Veritaban� ba�lant�s�n� yap�land�rma
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Connection string'i ayarlay�n

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Oturum middleware'�n� buraya ta��d�k
app.UseSession();

// Kullan�c� giri� kontrol� middleware'�
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    // Giri� yap�lmam��sa ve giri� sayfas� de�ilse
    if (string.IsNullOrEmpty(context.Session.GetString("UserId")) && path != "/Account/Login" && context.Request.Method != "POST")
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next(); // Di�er middleware'lar� �a��r
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
