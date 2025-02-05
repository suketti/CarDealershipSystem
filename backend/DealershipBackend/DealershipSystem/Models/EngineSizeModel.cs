using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DealershipSystem.Models;


public class EngineSizeModel
{
    public int ID { get; set; }

    [Required]
    public int ModelID { get; set; }

    [Range(300, 10000, ErrorMessage = "Invalid")]
    public int EngineSize { get; set; }

    public virtual FuelType FuelType { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual CarModel CarModel { get; set; } = null!;
}