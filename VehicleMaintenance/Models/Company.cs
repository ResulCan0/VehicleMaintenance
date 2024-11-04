using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Middleware;

public class Company
{
    public Company() { }

    private string _companyName;

    [Key]
    public Guid CompanyId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CompanyName
    {
        get => _companyName;
        set
        {
            _companyName = value;
            if (CompanyId == Guid.Empty)
            {
                CompanyId = GuidCreate.GenerateGuidFromUsername(value);
            }
        }
    }

    [Required]
    [MaxLength(15)]
    public string TaxNumber { get; set; }

    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ICollection<Vehicle>? Vehicles { get; set; }
    public ICollection<User>? CompanyUsers { get; set; }
    public ICollection<CompanyModule>? CompanyModules { get; set; }
}
