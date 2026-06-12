using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Setting")]
public class Setting
{
    [Key]
    [StringLength(50)]
    public string Parameter { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Value { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }
}
