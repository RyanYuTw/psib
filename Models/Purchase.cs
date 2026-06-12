using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Purchase")]
public class Purchase
{
    [Key]
    [StringLength(12)]
    public string Id { get; set; } = string.Empty;

    public DateTime PurchaseDate { get; set; } = DateTime.Now;

    [Required]
    [StringLength(10)]
    public string VendorId { get; set; } = string.Empty;

    [StringLength(5)]
    public string? CurrId { get; set; }

    [Column(TypeName = "decimal(10,4)")]
    public decimal ExcRate { get; set; } = 1;

    [Column(TypeName = "decimal(12,2)")]
    public decimal TaxRate { get; set; } = 5;

    [Column(TypeName = "decimal(12,2)")]
    public decimal SubTotal { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Tax { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Total { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Paid { get; set; } = 0;

    [StringLength(4)]
    public string? EmployeeNo { get; set; }

    public bool Deleted { get; set; } = false;

    [StringLength(500)]
    public string? Memo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("VendorId")]
    public Vendor? Vendor { get; set; }

    public ICollection<PurchaseDetail> Details { get; set; } = new List<PurchaseDetail>();
    public ICollection<AccountPayable> AccountPayables { get; set; } = new List<AccountPayable>();
}
