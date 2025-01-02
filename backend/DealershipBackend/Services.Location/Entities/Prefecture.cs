using System.ComponentModel.DataAnnotations;

namespace Services.Location.Entities;

public class Prefecture
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string NameJP { get; set; }
}
