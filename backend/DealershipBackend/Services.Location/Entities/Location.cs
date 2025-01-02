using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Location.Entities;

public class Location
{
    public int ID { get; set; }

    [Required]
    public string LocationName { get; set; }

    [Required]
    public int AddressId { get; set; } 
    
    [Required]
    [ForeignKey("AddressId")]
    public virtual Address Address { get; set; }

    [Required]
    public string PhoneNumber { get; set; }
}
