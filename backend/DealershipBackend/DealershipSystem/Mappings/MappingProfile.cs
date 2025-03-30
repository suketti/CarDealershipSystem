using AutoMapper;
using DealershipSystem.DTO;
using DealershipSystem.Models;


namespace DealershipSystem.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Forward mappings
        CreateMap<Location, LocationDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<Prefecture, PrefectureDTO>();

        CreateMap<User, UserDTO>();
        CreateMap<BodyType, BodyTypeDTO>();
        CreateMap<TransmissionType, TransmissionTypeDTO>();
        CreateMap<FuelType, FuelTypeDTO>();
        CreateMap<DrivetrainType, DrivetrainTypeDTO>();
        CreateMap<Color, ColorDTO>();
        CreateMap<CarMaker, CarMakerDTO>();
        CreateMap<CarModel, CarModelDTO>();
        CreateMap<EngineSizeModel, EngineSizeModelDTO>();
        CreateMap<Car, CarDTO>();
        CreateMap<Image, ImageDTO>();

        //Reverse mappings
        CreateMap<LocationDto, Location>();
        CreateMap<AddressDto, Address>();
        CreateMap<PrefectureDTO, Prefecture>();
        CreateMap<UserDTO, User>();
        CreateMap<BodyTypeDTO, BodyType>();
        CreateMap<TransmissionTypeDTO, TransmissionType>();
        CreateMap<FuelTypeDTO, FuelType>();
        CreateMap<DrivetrainTypeDTO, DrivetrainType>();
        CreateMap<CarMakerDTO, CarMaker>();
        CreateMap<UpdateCarModelDTO, CarModel>();
        CreateMap<CreateCarDTO, Car>()
        .ForMember(dest => dest.BrandID, opt => opt.MapFrom(src => src.Brand))
        .ForMember(dest => dest.ModelID, opt => opt.MapFrom(src => src.Model))
        .ForMember(dest => dest.BodyTypeID, opt => opt.MapFrom(src => src.BodyType)) 
        .ForMember(dest => dest.LocationID, opt => opt.MapFrom(src => src.Location))
        .ForMember(dest => dest.EngineSizeID, opt => opt.MapFrom(src => src.EngineSize))
        .ForMember(dest => dest.FuelTypeID, opt => opt.MapFrom(src => src.FuelType))
        .ForMember(dest => dest.DriveTrainID, opt => opt.MapFrom(src => src.DriveTrain))
        .ForMember(dest => dest.TransmissionTypeID, opt => opt.MapFrom(src => src.TransmissionType))
        .ForMember(dest => dest.ColorID, opt => opt.MapFrom(src => src.Color))
        .ForMember(dest => dest.LicensePlateNumber, opt => opt.MapFrom(src => src.LicensePlateNumber))
        .ForMember(dest => dest.RepairHistory, opt => opt.MapFrom(src => src.RepairHistory))
        .ForMember(dest => dest.MOTExpiry, opt => opt.MapFrom(src => src.MOTExpiry))
        .ForMember(dest => dest.VINNum, opt => opt.MapFrom(src => src.VINNum))
        .ForMember(dest => dest.IsSmoking, opt => opt.MapFrom(src => src.IsSmoking))
        .ForMember(dest => dest.IsInTransfer, opt => opt.MapFrom(src => src.IsInTransfer))
        .ForMember(dest => dest.CarExtras, opt => opt.MapFrom(src => 
            src.Extras != null ? src.Extras.Select(id => new CarExtra { ExtraID = id }).ToList() : new List<CarExtra>()))
            
        // Ignore navigation properties (they are set manually later)
        .ForMember(dest => dest.Brand, opt => opt.Ignore()) 
        .ForMember(dest => dest.CarModel, opt => opt.Ignore()) 
        .ForMember(dest => dest.BodyType, opt => opt.Ignore()) 
        .ForMember(dest => dest.Location, opt => opt.Ignore()) 
        .ForMember(dest => dest.EngineSize, opt => opt.Ignore()) 
        .ForMember(dest => dest.FuelType, opt => opt.Ignore()) 
        .ForMember(dest => dest.DriveTrain, opt => opt.Ignore()) 
        .ForMember(dest => dest.TransmissionType, opt => opt.Ignore()) 
        .ForMember(dest => dest.Color, opt => opt.Ignore()); 
        }
}