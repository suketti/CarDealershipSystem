using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealershipSystem.Models;

public class Extra
{
    [Key]
    public int ID { get; set; }

    [Required]
    [ForeignKey("ExtraType")]
    public int Type { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty!;

    [Required]
    public string NameJP { get; set; } = string.Empty!;

    // Navigation property
    public virtual ExtraType ExtraType { get; set; } = null!;
}