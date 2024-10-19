using System.ComponentModel.DataAnnotations;

public class StockPart
{
    [Key]
    public Guid PartId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PartNumber { get; set; }

    [MaxLength(50)]
    public string PartName { get; set; }

    public DateTime PartDate { get; set; }

    public int PartAmount { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stok eksi olamaz.")]
    public int StockQuantity { get; set; }
}
