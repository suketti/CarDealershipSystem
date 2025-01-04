using System.ComponentModel.DataAnnotations;

namespace DealershipSystem;

public class PrefectureDTO
{
    [Required]
    [MaxLength(15)]
    public string Name { get; set; }
    [Required]
    [MaxLength(4)]
    public string NameJP { get; set; }
}