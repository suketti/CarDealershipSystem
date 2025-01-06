using System.ComponentModel.DataAnnotations;
using DealershipSystem.Models;

namespace DealershipSystem.DTO;

public class UserDTO
{
    public Guid ID { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(15)]
    public string NameKanji { get; set; }
    [Required]
    [MaxLength(254)]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    [MaxLength(2)]
    [RegularExpression("^(en|jp|hu)$", ErrorMessage = "PreferredLanguage must be 'en', 'jp' or 'hu'.")]
    public string PreferredLanguage { get; set; }
}