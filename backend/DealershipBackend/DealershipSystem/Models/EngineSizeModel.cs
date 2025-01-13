namespace DealershipSystem.Models;

public class EngineSizeModel
{
    public int ID { get; set; }
    public int ModelID { get; set; }
    public int EngineSize { get; set; }

    // Navigation properties
    public virtual CarModel CarModel { get; set; } = null!;
}