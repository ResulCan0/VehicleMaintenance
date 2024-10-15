using System.ComponentModel.DataAnnotations;

public class Vehicle
{
    [Key]
    public Guid VehicleId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PlateNumber { get; set; }

    [MaxLength(50)]
    public string VehicleType { get; set; }

    public int Year { get; set; }

    public Guid BrandModelId { get; set; }
    public BrandModel BrandModel { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; }

    public ICollection<VehicleMaintenance> VehicleMaintenances { get; set; }
    public ICollection<VehiclePart> VehicleParts { get; set; }
}
