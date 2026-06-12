using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Currency")]
public class Currency
{
    [Key]
    [StringLength(5)]
    public string CurrId { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,4)")]
    public decimal ExcRate { get; set; } = 1;
}
