using System.ComponentModel.DataAnnotations;

namespace Services.Location.DTO;

#nullable disable

public class LocationDto
{
    public int Id { get; set; }
    [Required]
    public string LocationName { get; set; }

    [Required]
    public AddressDto Address { get; set; }
    [Required]
    [MaxLength(15)]
    public required string PhoneNumber { get; set; }
    
}
