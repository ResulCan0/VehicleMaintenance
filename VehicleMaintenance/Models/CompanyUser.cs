using System.ComponentModel.DataAnnotations;

public class CompanyUser
{
    [Key]
    public Guid CompanyUserId { get; set; }

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; } // Şirkete referans

    public Guid UserId { get; set; }
    public User? User { get; set; } // Kullanıcıya referans

    [Required]
    public string Role { get; set; } // Kullanıcının şirketteki rolü (örn. Yönetici, Çalışan)
}
