using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Unit")]
public class Unit
{
    [Key]
    [StringLength(10)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Name { get; set; } = string.Empty;
}
