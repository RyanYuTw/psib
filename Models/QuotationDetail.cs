using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Quotation_detail")]
public class QuotationDetail
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(12)]
    public string QuotationId { get; set; } = string.Empty;

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

    [StringLength(200)]
    public string? Memo { get; set; }

    [ForeignKey("QuotationId")]
    public Quotation? Quotation { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
}
