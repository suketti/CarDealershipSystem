using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DealershipSystem.Models;

public class User : IdentityUser
{
    [Required]
    [MaxLength(70)]
    public string Name { get; set; }
    [MaxLength(15)]
    public string NameKanji { get; set; }
    [Required]
    [MaxLength(2)]
    public string PreferredLanguage { get; set; }
}