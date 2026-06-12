using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Warehouse_stock")]
public class WarehouseStock
{
    public int WarehouseId { get; set; }

    [StringLength(20)]
    public string ProductId { get; set; } = string.Empty;

    [Column(TypeName = "decimal(12,2)")]
    public decimal OpeningStock { get; set; } = 0;

    [Column(TypeName = "decimal(12,4)")]
    public decimal OpeningCost { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal SafeVolumn { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal CurrentVolumn { get; set; } = 0;

    [ForeignKey("WarehouseId")]
    public Warehouse? Warehouse { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
}
