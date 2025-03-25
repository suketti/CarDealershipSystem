namespace DealershipSystem.DTO
{
    public class CreateCarDTO
    {
        public int Brand { get; set; } // Refers to CarMaker.ID
        public int Model { get; set; } // Refers to Models.ID
        public string Grade { get; set; } = string.Empty; // Grade as string (A, B, C, etc.)
        public int BodyType { get; set; } // Refers to BodyTypes.ID
        public int Location { get; set; } // Refers to Locations.ID
        public int EngineSize { get; set; } // Refers to EngineSizeModel.ID
        public string LicensePlateNumber { get; set; } // Optional
        public bool RepairHistory { get; set; } = false; // Defaults to false
        public int FuelType { get; set; } // Refers to FuelTypes.ID
        public int DriveTrain { get; set; } // Refers to DrivetrainTypes.ID
        public DateTime? MOTExpiry { get; set; } // Optional
        public int TransmissionType { get; set; } // Refers to TransmissionTypes.ID
        public string VINNum { get; set; } = string.Empty; // Vehicle Identification Number
        public int Color { get; set; } // Refers to Colors.ID
        public string? Price { get; set; } // Price of the car
        public int? Mileage { get; set; } //Optional price
        public bool IsSmoking { get; set; } = false; // Defaults to false
        public List<int> Extras { get; set; } // List of Extra.ID values (Optional)
        public bool IsInTransfer { get; set; } = false; // Defaults to false
    }
}