using System.ComponentModel.DataAnnotations;

namespace Services.Location.Entities;

public class Prefecture
{
    public int Id { get; set; }
    [Required]
    [MaxLength(15)]
    public string Name { get; set; }
    [Required]
    [MaxLength(4)]
    public string NameJP { get; set; }
}
