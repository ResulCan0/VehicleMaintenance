using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.Models
{
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } // Rolün ismi (eski "Roles")

        // Bir Role, birçok kullanıcıya sahip olabilir.
        public ICollection<User>? Users { get; set; } = new List<User>();
    }
}
