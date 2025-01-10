using System.ComponentModel.DataAnnotations;

namespace DealershipSystem;

public class LocationDto
{
    public int Id { get; set; }
    [Required]
    public string LocationName { get; set; }

    [Required]
    public AddressDto Address { get; set; }
    [Required]
    public int MaxCapacity { get; set; }
    [Required]
    [MaxLength(15)]
    public required string PhoneNumber { get; set; }
    
}
