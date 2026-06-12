using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("UserGroup")]
public class UserGroup
{
    [Key]
    [StringLength(10)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool CanSale { get; set; } = true;
    public bool CanPurchase { get; set; } = true;
    public bool CanReport { get; set; } = true;
    public bool CanSetting { get; set; } = false;
    public bool CanUserMgmt { get; set; } = false;

    public ICollection<User> Users { get; set; } = new List<User>();
}
