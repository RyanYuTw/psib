using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIB.Models;

[Table("Reminder")]
public class Reminder
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Content { get; set; }

    public DateTime RemindDate { get; set; }

    public bool IsCompleted { get; set; } = false;

    [StringLength(4)]
    public string? EmployeeNo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
