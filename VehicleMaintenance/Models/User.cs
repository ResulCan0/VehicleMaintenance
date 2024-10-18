using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleMaintenance.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }  // Kullanıcı ID'si
    public Guid RoleId { get; set; }
    public Role? Roles { get; set; }
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }  // Kullanıcı adı

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }  // Kullanıcı soyadı

    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }  // Kullanıcı e-posta adresi

    [MaxLength(15)]
    [Phone]
    public string PhoneNumber { get; set; }  // Kullanıcı telefon numarası

    [Required]
    public string? Password { get; set; }  // Şifre (hashlenmiş)

    public bool IsActive { get; set; }  // Kullanıcı aktif/pasif durumu
    
    public Guid CompanyId { get; set; } // Yabancı anahtar
    public Company? Company { get; set; }
    
}
