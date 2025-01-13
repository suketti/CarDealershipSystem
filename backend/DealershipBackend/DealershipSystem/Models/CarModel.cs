namespace DealershipSystem.Models;

public class CarModel
{
    public int ID { get; set; }
    public int MakerID { get; set; }
    public string ModelNameJapanese { get; set; } = string.Empty!;
    public string ModelNameEnglish { get; set; } = string.Empty!;
    public string ModelCode { get; set; } = string.Empty!;
    public int ManufacturingStartYear { get; set; }
    public int ManufacturingEndYear { get; set; }
    public int PassengerCount { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Mass { get; set; }

    // Navigation properties
    public virtual CarMaker Maker { get; set; } = null!;
    public virtual ICollection<EngineSizeModel> EngineSizes { get; set; } = new List<EngineSizeModel>();
}