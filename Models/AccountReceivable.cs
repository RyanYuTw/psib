using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("AccountReceivable")]
public class AccountReceivable
{
    [Key]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [StringLength(12)]
    public string? SaleId { get; set; }

    public DateTime? ReceiveDate { get; set; }

    [StringLength(10)]
    public string? CustomerId { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal ReceiveCash { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal ReceiveCheck { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Allowance { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Fee { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Others { get; set; } = 0;

    [StringLength(1)]
    public string? PreReceivedType { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal PreReceived { get; set; } = 0;

    [StringLength(10)]
    public string? BankId { get; set; }

    public DateTime? DueDate { get; set; }

    [StringLength(50)]
    public string? Account { get; set; }

    [StringLength(30)]
    public string? CheckNo { get; set; }

    [StringLength(500)]
    public string? Memo { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal ReceiveAmount { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal TotalBalance { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("SaleId")]
    public Sale? Sale { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    [ForeignKey("BankId")]
    public Bank? Bank { get; set; }
}
