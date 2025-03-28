using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealershipSystem.Models;

[Table("EmployeeLocations")]
public class EmployeeLocation
{
    [Key]
    public int Id { get; set; } // Primary key

    [Required]
    public int EmployeeId { get; set; } // Employee ID

    [Required]
    public string Location { get; set; } // Assigned location
}