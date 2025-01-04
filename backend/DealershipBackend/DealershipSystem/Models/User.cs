using System.ComponentModel.DataAnnotations;

namespace DealershipSystem.Models;

public class User
{
    [Key] public Guid ID { get; set; } = Guid.NewGuid();
    [Required]
    [MaxLength(70)]
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