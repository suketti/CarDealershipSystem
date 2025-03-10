using System.ComponentModel.DataAnnotations;

namespace DealershipSystem.DTO;

public class CreateReservationDTO
{
    [Required]
    public Guid UserId { get; set; }
        
    [Required]
    public int CarId { get; set; }
        
    [Required]
    public DateTime Date { get; set; }
}