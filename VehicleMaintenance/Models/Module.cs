using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Module
{
    [Key]
    public Guid ModuleId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ModuleName { get; set; }

    [Required]
    public int ModuleCode { get; set; } // Modül kodu

    public ICollection<CompanyModule>? CompanyModules { get; set; } // Modülün hangi şirketlere atandığı
}
