using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Quotation")]
public class Quotation
{
    [Key]
    [StringLength(12)]
    public string Id { get; set; } = string.Empty;

    public DateTime QuotationDate { get; set; } = DateTime.Now;

    [Required]
    [StringLength(10)]
    public string CustomerId { get; set; } = string.Empty;

    public DateTime? ValidDate { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal SubTotal { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Tax { get; set; } = 0;

    [Column(TypeName = "decimal(12,2)")]
    public decimal Total { get; set; } = 0;

    [StringLength(4)]
    public string? EmployeeNo { get; set; }

    public bool Deleted { get; set; } = false;

    [StringLength(500)]
    public string? Memo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public ICollection<QuotationDetail> Details { get; set; } = new List<QuotationDetail>();
}
