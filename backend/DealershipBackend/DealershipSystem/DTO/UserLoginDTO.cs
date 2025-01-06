using System.ComponentModel.DataAnnotations;

namespace DealershipSystem.DTO;

public class UserLoginDTO
{
    [Required]
    [MaxLength(254)]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}