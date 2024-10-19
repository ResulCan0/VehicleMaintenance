using System.ComponentModel.DataAnnotations;

public class CompanyModule
{
    [Key]
    public Guid ModuleId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ModuleName { get; set; }

    [Required]
    public int ModuleCode { get; set; }
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
}
