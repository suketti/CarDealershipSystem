using DealershipSystem.Models;

namespace DealershipSystem.DTO;

public class CarDTO
{
    public int ID { get; set; }
    public CarMakerDTO Brand { get; set; } = null!;
    public ModelDTO Model { get; set; } = null!;
    public string Grade { get; set; } = string.Empty!;
    public BodyTypeDTO BodyType { get; set; } = null!;
    public LocationDto Location { get; set; } = null!;
    public EngineSizeModelDTO EngineSize { get; set; } = null!;
    public string? LicensePlateNumber { get; set; }
    public bool RepairHistory { get; set; }
    public FuelTypeDTO FuelType { get; set; } = null!;
    public DrivetrainTypeDTO DriveTrain { get; set; } = null!;
    public DateTime? MOTExpiry { get; set; }
    public TransmissionTypeDTO TransmissionType { get; set; } = null!;
    public int VINNum { get; set; }
    public ColorDTO Color { get; set; } = null!;
    public bool IsSmoking { get; set; }
    public List<CarExtraDTO> Extras { get; set; } = new List<CarExtraDTO>();
    //public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    public bool IsInTransfer { get; set; }
}