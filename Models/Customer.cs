using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Customer")]
public class Customer
{
    [Key]
    [StringLength(10)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(8)]
    public string? BusinessNo { get; set; }

    [StringLength(10)]
    public string? GroupId { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(20)]
    public string? Fax { get; set; }

    [StringLength(20)]
    public string? Cell { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Contact { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal CreditLimit { get; set; } = 0;

    [StringLength(5)]
    public string? CurrId { get; set; }

    public bool IsActive { get; set; } = true;

    [StringLength(500)]
    public string? Memo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ICollection<AccountReceivable> AccountReceivables { get; set; } = new List<AccountReceivable>();
}
