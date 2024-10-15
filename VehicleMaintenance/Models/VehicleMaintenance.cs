using System.ComponentModel.DataAnnotations;

public class VehicleMaintenances
{
    [Key]
    public Guid VehicleMaintenanceId { get; set; }

    [Required]
    [MaxLength(50)]
    public string MaintenanceType { get; set; }

    [MaxLength(50)]
    public string ReasonCode { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    public decimal ChargeParts { get; set; }

    public Guid VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
}
