using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealershipSystem.Models;

[Table("EmployeeLocations")]
public class EmployeeLocation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }  // Primary key

    [Required]
    public Guid EmployeeId { get; set; }  // Employee ID (string as IdentityUser uses string)

    [Required]
    public int LocationId { get; set; }  // Assigned Location ID (foreign key to Location table)

    // Navigation property to the Location
    [ForeignKey("LocationId")]
    public virtual Location Location { get; set; }
}