using System.ComponentModel.DataAnnotations;

namespace DealershipSystem.DTO
{
    public class ChangePasswordAdminDTO
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}