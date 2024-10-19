using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using VehicleMaintenance.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<BrandModel> BrandModels { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleMaintenances> VehicleMaintenances { get; set; }
    public DbSet<VehiclePart> VehicleParts { get; set; }
    public DbSet<StockPart> StockParts { get; set; }
    public DbSet<User> CompanyUsers { get; set; }
    public DbSet<Role> Roles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<User>()
      .HasOne(u => u.Company) // Kullanıcıdan şirkete bir ilişki
      .WithMany(c => c.CompanyUsers) // Şirketten kullanıcılara birden çok ilişki
      .HasForeignKey(u => u.CompanyId)
      .OnDelete(DeleteBehavior.Restrict);//
    }
}
