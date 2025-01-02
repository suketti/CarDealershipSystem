using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Location.Entities;

public class Address
{
    public int Id { get; set; }

    [Required]
    public string PostalCode { get; set; }

    [Required]
    public int PrefectureId { get; set; }

    [Required]
    public virtual Prefecture Prefecture { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string CityRomanized { get; set; }

    [Required]
    public string Street { get; set; }

    [Required]
    public string StreetRomanized { get; set; }
}
