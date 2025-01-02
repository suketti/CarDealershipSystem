using System.ComponentModel.DataAnnotations;

namespace Services.Location.Entities;

public class Location
{
    public int ID { get; set; }

    [Required]
    public string LocationName { get; set; }

    [Required]
    public Address Address { get; set; }

    [Required]
    public string PhoneNumber { get; set; }
}
