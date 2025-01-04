using System.ComponentModel.DataAnnotations;

namespace DealershipSystem;

public class AddressDto
{
    [MaxLength(8)]
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
