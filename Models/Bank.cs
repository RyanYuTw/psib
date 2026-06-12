using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Bank")]
public class Bank
{
    [Key]
    [StringLength(10)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(10)]
    public string? BranchCode { get; set; }

    [StringLength(50)]
    public string? BranchName { get; set; }

    [StringLength(30)]
    public string? AccountNo { get; set; }
}
