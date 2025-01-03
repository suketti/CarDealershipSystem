using System.ComponentModel.DataAnnotations;
using Services.Location.Entities;

namespace Services.Location.DTO;

public class AddressDto
{
    [MaxLength(7)]
    [Required]  
    public string PostalCode { get; set; }

    [Required]  
    public PrefectureDTO Prefecture { get; set; }

    [Required]  
    public string City { get; set; }

    [Required] 
    public string CityRomanized { get; set; }

    [Required]  
    public string Street { get; set; }

    [Required]
    public string StreetRomanized { get; set; }
}
