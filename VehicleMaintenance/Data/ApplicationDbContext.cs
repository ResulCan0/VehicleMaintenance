using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection.Emit;
using VehicleMaintenance.Models;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    
    public DbSet<Company> Companies { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<BrandModel> BrandModels { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleMaintenances> VehicleMaintenances { get; set; }
    public DbSet<VehiclePart> VehicleParts { get; set; }
    public DbSet<StockPart> StockParts { get; set; }
    public DbSet<User> CompanyUsers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<CompanyModule> CompanyModules { get; set; }
    public DbSet<Module> Modules { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
     : base(options)
    {
        _configuration = configuration;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
       .HasOne(u => u.Company) // Kullanıcıdan şirkete bir ilişki
       .WithMany(c => c.CompanyUsers) // Şirketten kullanıcılara birden çok ilişki
       .HasForeignKey(u => u.CompanyId)
       .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
        .HasOne(u => u.Roles)  // Kullanıcıdan role bir ilişki
        .WithMany(r => r.Users)  // Role'den kullanıcılara birden çok ilişki
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompanyModule>()
        .HasOne(u => u.Company)
        .WithMany(c => c.CompanyModules)
        .HasForeignKey(u => u.CompanyId)
        .OnDelete(DeleteBehavior.Restrict);

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // application.json'dan şifrelenmiş bağlantı dizisini al
        var encryptedConnectionString = _configuration.GetConnectionString("DefaultConnection");

        // Şifrelenmiş bağlantı dizisini çöz
        var decryptedConnectionString = EncryptionService.Decrypt(encryptedConnectionString);

        // Çözülmüş bağlantı dizisini kullanarak veritabanına bağlan
        optionsBuilder.UseSqlServer(decryptedConnectionString);
    }
}
