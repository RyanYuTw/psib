using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("AccountPayable")]
public class AccountPayable
{
    [Key]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [StringLength(12)]
    public string? PurchaseId { get; set; }

    public DateTime? PayDate { get; set; }

    [StringLength(10)]
    public string? VendorId { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal PayCash { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal PayCheck { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Allowance { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Fee { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Others { get; set; } = 0;

    [StringLength(1)]
    public string? PrePaidType { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal PrePaid { get; set; } = 0;

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
    public decimal PayAmount { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal TotalBalance { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("PurchaseId")]
    public Purchase? Purchase { get; set; }

    [ForeignKey("VendorId")]
    public Vendor? Vendor { get; set; }

    [ForeignKey("BankId")]
    public Bank? Bank { get; set; }
}
