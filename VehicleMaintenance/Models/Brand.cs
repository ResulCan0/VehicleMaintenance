using System.ComponentModel.DataAnnotations;

public class Brand
{
    [Key]
    public Guid BrandId { get; set; }

    [Required]
    [MaxLength(50)]
    public string BrandName { get; set; }

    public ICollection<BrandModel> BrandModels { get; set; }
}
