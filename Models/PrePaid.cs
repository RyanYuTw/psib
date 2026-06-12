using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("PrePaid")]
public class PrePaid
{
    [Key]
    [StringLength(12)]
    public string Id { get; set; } = string.Empty;

    [StringLength(12)]
    public string? PurchaseId { get; set; }

    [StringLength(10)]
    public string? VendorId { get; set; }

    public DateTime OccurDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; } = 0;

    [ForeignKey("VendorId")]
    public Vendor? Vendor { get; set; }
}
