using System.ComponentModel.DataAnnotations;

namespace Services.Location.DTO;

public class PrefectureDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string NameJP { get; set; }
}