using System.ComponentModel.DataAnnotations;

namespace DealershipSystem.DTO
{
    public class ChangePassswordDTO
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}