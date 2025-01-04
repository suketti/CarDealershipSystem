using AutoMapper;


namespace DealershipSystem.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Forward mappings
        CreateMap<Models.Location, LocationDto>();
        CreateMap<Models.Address, AddressDto>();
        CreateMap<Models.Prefecture, PrefectureDTO>();
        //Reverse mappings
        CreateMap<LocationDto, Models.Location>();
        CreateMap<AddressDto, Models.Address>();
        CreateMap<PrefectureDTO, Models.Prefecture>();
    }
}