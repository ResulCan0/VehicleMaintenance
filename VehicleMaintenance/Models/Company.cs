using System.ComponentModel.DataAnnotations;

public class Company
{
    [Key]
    public Guid CompanyId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CompanyName { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; }
}
