using System.ComponentModel.DataAnnotations;

public class CompanyModule
{
    [Key]
    public Guid CompanyModuleId { get; set; }

    [Required]
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }

    [Required]
    public Guid ModuleId { get; set; }
    public Module Module { get; set; }

    public DateTime AssignedDate { get; set; } = DateTime.Now; // Modül atama tarihi
    public bool IsActive { get; set; } = true; // Modül aktif mi
}
