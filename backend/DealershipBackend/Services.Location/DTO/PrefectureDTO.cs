using System.ComponentModel.DataAnnotations;

namespace Services.Location.DTO;

public class PrefectureDTO
{
    [Required]
    [MaxLength(15)]
    public string Name { get; set; }
    [Required]
    [MaxLength(4)]
    public string NameJP { get; set; }
}