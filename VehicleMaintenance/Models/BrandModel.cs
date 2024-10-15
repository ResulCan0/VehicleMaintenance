using System.ComponentModel.DataAnnotations;

public class BrandModel
{
    [Key]
    public Guid BrandModelId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ModelName { get; set; }

    [MaxLength(50)]
    public string Class { get; set; }

    public Guid BrandId { get; set; }
    public Brand? Brand { get; set; }

    public ICollection<Vehicle>? Vehicles { get; set; }
}
