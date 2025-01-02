using System.ComponentModel.DataAnnotations;

namespace Services.Location.DTO;

#nullable disable

public class LocationDto
{
    [Required]
    public string LocationName { get; set; }
    [Required]
    public AddressDto Address { get; set; }
    [Required]
    public required string PhoneNumber { get; set; }
    
}
