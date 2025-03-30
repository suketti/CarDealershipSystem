using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WpfApp1.Models
{
    public class CarMakerDTO
    {
        public int ID { get; set; }
        public string BrandEnglish { get; set; } = string.Empty;
        public string BrandJapanese { get; set; } = string.Empty;
    }

    public class CarModelDTO
    {
        public int ID { get; set; }
        public int MakerID { get; set; }
        public string ModelNameEnglish { get; set; }
        public string ModelNameJapanese { get; set; }
        public string ModelCode { get; set; }
        public int ManufacturingStartYear { get; set; }
        public int ManufacturingEndYear { get; set; }
        public int PassengerCount { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mass { get; set; }
        public CarMakerDTO Maker { get; set; }
        public List<EngineSizeModelDTO> EngineSizes { get; set; }
    }

    public class ColorDTO
    {
        public int ID { get; set; }
        public string ColorNameEnglish { get; set; } = string.Empty;
        public string ColorNameJapanese { get; set; } = string.Empty;
    }

    public class FuelTypeDTO
    {
        public int ID { get; set; }
        public string NameJapanese { get; set; } = string.Empty;
        public string NameEnglish { get; set; } = string.Empty;
    }

    public class EngineSizeModelDTO
    {
        public int ID { get; set; }
        public FuelTypeDTO FuelType { get; set; }
        public int EngineSize { get; set; }

        public EngineSizeModelDTO()
        {
            FuelType = new FuelTypeDTO();
        }

        public string DisplayText => $"{EngineSize} ({FuelType?.NameEnglish})";
    }

    public class CarExtraDTO
    {
        public int CarExtraID { get; set; }
        public int CarID { get; set; }
        public int ExtraID { get; set; }
        public string ExtraName { get; set; } = string.Empty;
    }

    public class ImageDTO
    {
        public string URL { get; set; } = string.Empty;
    }

    public class PrefectureDTO
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(4)]
        public string NameJP { get; set; } = string.Empty;
    }

    public class AddressDTO
    {
        [MaxLength(8)]
        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public PrefectureDTO Prefecture { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string CityRomanized { get; set; } = string.Empty;

        [Required]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string StreetRomanized { get; set; } = string.Empty;

        public AddressDTO()
        {
            Prefecture = new PrefectureDTO();
        }
    }

    public class LocationDTO
    {
        public int Id { get; set; }

        [Required]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        public AddressDTO Address { get; set; }

        [Required]
        public int MaxCapacity { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        public LocationDTO()
        {
            Address = new AddressDTO();
        }
    }

    public class CarDTO
    {
        public int ID { get; set; }
        public CarMakerDTO Brand { get; set; }
        public CarModelDTO CarModel { get; set; }
        public string Grade { get; set; } = string.Empty;
        public BodyTypeDTO BodyType { get; set; }
        public LocationDTO Location { get; set; }
        public EngineSizeModelDTO EngineSize { get; set; }
        public string LicensePlateNumber { get; set; }
        public bool RepairHistory { get; set; }
        public FuelTypeDTO FuelType { get; set; }
        public DrivetrainTypeDTO DriveTrain { get; set; }
        public DateTime? MOTExpiry { get; set; }
        public TransmissionTypeDTO TransmissionType { get; set; }
        public int Mileage { get; set; }
        public string VINNum { get; set; } = string.Empty;
        public ColorDTO Color { get; set; }
        public bool IsSmoking { get; set; }
        public string Price { get; set; }
        public List<CarExtraDTO> Extras { get; set; }
        public List<ImageDTO> Images { get; set; }
        public bool IsInTransfer { get; set; }

        public CarDTO()
        {
            Brand = new CarMakerDTO();
            CarModel = new CarModelDTO();
            BodyType = new BodyTypeDTO();
            Location = new LocationDTO();
            EngineSize = new EngineSizeModelDTO();
            FuelType = new FuelTypeDTO();
            DriveTrain = new DrivetrainTypeDTO();
            TransmissionType = new TransmissionTypeDTO();
            Color = new ColorDTO();
            Extras = new List<CarExtraDTO>();
            Images = new List<ImageDTO>();
        }
    }

    public class BodyTypeDTO
    {
        public int ID { get; set; }
        public string NameEnglish { get; set; }
        public string NameJapanese { get; set; }
    }

    public class DrivetrainTypeDTO
    {
        public int ID { get; set; }
        public string Type { get; set; }
    }

    public class TransmissionTypeDTO
    {
        public int ID { get; set; }
        public string Type { get; set; } = string.Empty;
    }

    public class ExtraDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UserDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameKanji { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PreferredLanguage { get; set; } = string.Empty;
    }

    public class DealerLoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Role { get; set; }
        public LocationDTO Location { get; set; }
    }

    public class TokenResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class LocationResponseDTO
    {
        public int ID { get; set; }
        public string employeeID { get; set; }
        public int locationID { get; set; }
        public LocationDTO locationDTO { get; set; }
    }


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
        public int Mileage { get; set; }
        public string VINNum { get; set; } = string.Empty; // Vehicle Identification Number
        public int Color { get; set; } // Refers to Colors.ID
        public string Price { get; set; } // Price of the car
        public bool IsSmoking { get; set; } = false; // Defaults to false
        public List<int> Extras { get; set; } // List of Extra.ID values (Optional)
        public bool IsInTransfer { get; set; } = false; // Defaults to false
    }

    public class CreateCarMakerDTO
    {
        public string BrandEnglish { get; set; }
        public string BrandJapanese { get; set; }
    }
    public class CreateCarModelDTO
    {
        public int MakerID { get; set; }
        public string ModelNameEnglish { get; set; }
        public string ModelNameJapanese { get; set; }
        public string ModelCode { get; set; }
        public int ManufacturingStartYear { get; set; }
        public int ManufacturingEndYear { get; set; }
        public int PassengerCount { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mass { get; set; }
    }

    public class AdminUserCreateDTO
    {
        // User information
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; } // Name of the user
        public string NameKanji { get; set; } // Kanji name for the user
        public string PhoneNumber { get; set; } // Phone number of the user
        public string PreferredLanguage { get; set; } // Preferred language for the user

        // Role of the user (Dealer/Admin)
        public string Role { get; set; }

        // Location (only applicable for Dealer role, optional)
        public LocationDTO Location { get; set; }
    }

    public class AdminUserUpdateDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string NameKanji { get; set; }
        public string PhoneNumber { get; set; }
        public string PreferredLanguage { get; set; }
        public string Role { get; set; }
        public LocationDTO Location { get; set; }
    }

    public class DealerUserDTO : UserDTO
    {
        public LocationDTO locationDTO { get; set; }
    }
}
