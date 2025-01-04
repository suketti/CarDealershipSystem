using System.ComponentModel.DataAnnotations;
using DealershipSystem.Models;

namespace DealershipSystem.DTO;

public class UserDTO
{
    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(15)]
    public string NameKanji { get; set; }
    [Required]
    [MaxLength(254)]
    public string Email { get; set; }
    [Required]
    public UserEnum Role { get; set; }
    [Required]
    [MaxLength(2)]
    public string PreferredLanguage { get; set; }
}