using System.ComponentModel.DataAnnotations;
using DealershipSystem.Models;

namespace DealershipSystem.DTO;

public class UserRegisterDto
{
    [Key] 
    public Guid ID { get; set; } = Guid.NewGuid();
    [Required]
    [MaxLength(70)]
    public string Name { get; set; }
    [MaxLength(15)]
    public string NameKanji { get; set; }

    [Required] 
    public string UserName { get; set; }
    [Required]
    [MaxLength(254)]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    
    [Required]
    [MaxLength(2)]
    public string PreferredLanguage { get; set; }
}