using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Product")]
public class Product
{
    [Key]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(30)]
    public string? Barcode { get; set; }

    [StringLength(10)]
    public string? CategoryId { get; set; }

    [StringLength(10)]
    public string? UnitId { get; set; }

    [StringLength(20)]
    public string? Pack { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Cost { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Price { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal CurrentVol { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal SafeVol { get; set; } = 0;

    public bool Stock { get; set; } = true;

    public bool IsActive { get; set; } = true;

    [StringLength(500)]
    public string? Memo { get; set; }

    public DateTime? LastBuyDate { get; set; }
    public DateTime? LastSaleDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    [ForeignKey("UnitId")]
    public Unit? Unit { get; set; }

    public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
}
