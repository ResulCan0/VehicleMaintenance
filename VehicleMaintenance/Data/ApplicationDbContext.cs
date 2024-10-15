using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<BrandModel> BrandModels { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
    public DbSet<VehiclePart> VehicleParts { get; set; }
    public DbSet<StockPart> StockParts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships, keys, etc.
    }
}
