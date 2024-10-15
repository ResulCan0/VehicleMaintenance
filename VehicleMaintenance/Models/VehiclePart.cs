using System.ComponentModel.DataAnnotations;

public class VehiclePart
{
    [Key]
    public Guid VehiclePartId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PartNumber { get; set; }

    [MaxLength(50)]
    public string PartName { get; set; }

    public int PartAmount { get; set; }

    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
}
