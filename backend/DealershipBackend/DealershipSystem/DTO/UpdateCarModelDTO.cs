using System.ComponentModel.DataAnnotations;
using DealershipSystem.Models;

namespace DealershipSystem.DTO;

public class UpdateCarModelDTO
{
    [Required]
    public int MakerID { get; set; }

    [Required, MaxLength(100)]
    public string ModelNameJapanese { get; set; } = string.Empty!;

    [Required, MaxLength(100)]
    public string ModelNameEnglish { get; set; } = string.Empty!;

    [Required, MaxLength(50)]
    public string ModelCode { get; set; } = string.Empty!;

    [Range(1900, 2100, ErrorMessage = "Invalid")]
    public int ManufacturingStartYear { get; set; }

    [Range(1900, 2100, ErrorMessage = "Invalid")]
    public int ManufacturingEndYear { get; set; }

    [Range(1, 10, ErrorMessage = "Invalid")]
    public int PassengerCount { get; set; }

    [Range(1000, 10000, ErrorMessage = "Invalid")]
    public int Length { get; set; }

    [Range(1000, 3000, ErrorMessage = "Invalid")]
    public int Width { get; set; }

    [Range(1000, 3000, ErrorMessage = "Invalid")]
    public int Height { get; set; }

    [Range(500, 5000, ErrorMessage = "Invalid")]
    public int Mass { get; set; }
}