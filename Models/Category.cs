using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Category")]
public class Category
{
    [Key]
    [StringLength(10)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Memo { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
