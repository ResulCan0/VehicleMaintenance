using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BrandModel
{
    [Key]
    public Guid BrandModelId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ModelName { get; set; }

    [MaxLength(50)]
    public string Class { get; set; }

    public Guid? BrandId { get; set; }
    public Brand? Brand { get; set; }
    // Birleştirilmiş değer
    [NotMapped] // Bu property veritabanına kaydedilmeyecek
    public string BrandModelText
    {
        get
        {
            return Brand != null ? $"{Brand.BrandName} - {ModelName}" : ModelName;
        }
    }

    public ICollection<Vehicle>? Vehicles { get; set; }
}
