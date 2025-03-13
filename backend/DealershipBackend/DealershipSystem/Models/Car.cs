namespace DealershipSystem.Models;

public class Car
{
    public int ID { get; set; }
    public int BrandID { get; set; }
    public int ModelID { get; set; }
    public Grade Grade { get; set; }
    public int BodyTypeID { get; set; }
    public int LocationID { get; set; }
    public int EngineSizeID { get; set; }
    public string? LicensePlateNumber { get; set; }
    public bool RepairHistory { get; set; } = false;
    public int FuelTypeID { get; set; }
    public int DriveTrainID { get; set; }
    public DateTime? MOTExpiry { get; set; }
    public int TransmissionTypeID { get; set; }
    public string VINNum { get; set; } = string.Empty!;
    public int ColorID { get; set; }
    public bool IsSmoking { get; set; } = false;
    public int Mileage { get; set; } = 0;
    public bool IsInTransfer { get; set; } = false;
    public string? Price { get; set; }

    // Navigation properties
    public virtual CarMaker Brand { get; set; } = null!;
    public virtual CarModel CarModel { get; set; } = null!;
    public virtual BodyType BodyType { get; set; } = null!;
    public virtual Location Location { get; set; } = null!;
    public virtual EngineSizeModel EngineSize { get; set; } = null!;
    public virtual FuelType FuelType { get; set; } = null!;
    public virtual DrivetrainType DriveTrain { get; set; } = null!;
    public virtual TransmissionType TransmissionType { get; set; } = null!;
    
    public virtual Color Color { get; set; } = null!;
    public virtual ICollection<CarExtra> CarExtras { get; set; } = new List<CarExtra>();
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    public DateTime DateOfManufacture { get; set; }
}