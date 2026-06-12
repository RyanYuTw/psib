using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("User")]
public class User
{
    [Key]
    [StringLength(4)]
    public string EmployeeNo { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(10)]
    public string? UserGroupId { get; set; }

    public bool IsActive { get; set; } = true;

    [ForeignKey("UserGroupId")]
    public UserGroup? UserGroup { get; set; }
}
