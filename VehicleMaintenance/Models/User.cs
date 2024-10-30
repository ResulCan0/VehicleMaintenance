using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models;
using VehicleMaintenance.Middleware;

public class User
{
    public User()
    {
        // Yeni bir kullanıcı oluşturulurken Email boş değilse, UserId'yi atar.
        if (!string.IsNullOrEmpty(_userEmail))
        {
            UserId = GuidCreate.GenerateGuidFromUsername(_userEmail);
        }
    }

    [Key]
    public Guid UserId { get; set; } // Kullanıcı ID'si

    public Guid RoleId { get; set; }
    public Role? Roles { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }  // Kullanıcı adı

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }  // Kullanıcı soyadı

    private string _userEmail;

    [MaxLength(100)]
    [EmailAddress]
    public string Email
    {
        get => _userEmail;
        set
        {
            _userEmail = value;
            // UserId yalnızca yeni bir kullanıcı eklenirken atanır.
            if (UserId == Guid.Empty)
            {
                UserId = GuidCreate.GenerateGuidFromUsername(value);
            }
        }
    }

    [MaxLength(15)]
    [Phone]
    public string PhoneNumber { get; set; }  // Kullanıcı telefon numarası

    [Required]
    public string? Password { get; set; }  // Şifre (hashlenmiş)

    public bool IsActive { get; set; }  // Kullanıcı aktif/pasif durumu

    public Guid CompanyId { get; set; } // Yabancı anahtar
    public Company? Company { get; set; }
}
