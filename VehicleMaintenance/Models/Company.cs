using System.ComponentModel.DataAnnotations;

public class Company
{
    [Key]
    public Guid CompanyId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CompanyName { get; set; }

    [Required]
    [MaxLength(15)]
    public string TaxNumber { get; set; } // Vergi Numarası

    public bool IsActive { get; set; } // Şirketin aktif olup olmadığını belirler

    public ICollection<Vehicle>? Vehicles { get; set; }
    
    public ICollection<User>? CompanyUsers { get; set; } // Şirkete bağlı kullanıcılar
    
    public ICollection<CompanyModule>? CompanyModules { get; set; } //  Şirketin kullanabileceği modüller
}

