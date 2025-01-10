using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealershipSystem.Models;

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
    public int MaxCapacity { get; set; }
    
    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }
}
