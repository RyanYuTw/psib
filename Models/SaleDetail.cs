using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Sale_detail")]
public class SaleDetail
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(12)]
    public string SaleId { get; set; } = string.Empty;

    public int Seq { get; set; }

    [Required]
    [StringLength(20)]
    public string ProductId { get; set; } = string.Empty;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; } = 0;

    [Column(TypeName = "decimal(6,2)")]
    public decimal Discount { get; set; } = 100;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Price { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal LineTotal { get; set; } = 0;

    public bool Deleted { get; set; } = false;

    [StringLength(200)]
    public string? Memo { get; set; }

    [ForeignKey("SaleId")]
    public Sale? Sale { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
}
