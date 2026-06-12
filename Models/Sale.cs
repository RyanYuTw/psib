using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Sale")]
public class Sale
{
    [Key]
    [StringLength(12)]
    public string Id { get; set; } = string.Empty;

    public DateTime SaleDate { get; set; } = DateTime.Now;

    [Required]
    [StringLength(10)]
    public string CustomerId { get; set; } = string.Empty;

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
    public decimal Received { get; set; } = 0;

    [StringLength(4)]
    public string? EmployeeNo { get; set; }

    public bool Deleted { get; set; } = false;

    [StringLength(500)]
    public string? Memo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public ICollection<SaleDetail> Details { get; set; } = new List<SaleDetail>();
    public ICollection<AccountReceivable> AccountReceivables { get; set; } = new List<AccountReceivable>();
}
