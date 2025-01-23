using System.ComponentModel.DataAnnotations;

namespace DealershipSystem.Models;

public class ExtraType
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty!;
}