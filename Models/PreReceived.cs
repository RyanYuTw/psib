using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("PreReceived")]
public class PreReceived
{
    [Key]
    [StringLength(12)]
    public string Id { get; set; } = string.Empty;

    [StringLength(12)]
    public string? SaleId { get; set; }

    [StringLength(10)]
    public string? CustomerId { get; set; }

    public DateTime OccurDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; } = 0;

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }
}
