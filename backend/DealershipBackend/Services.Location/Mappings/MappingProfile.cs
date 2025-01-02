using AutoMapper;
using Services.Location.DTO;

namespace Services.Location.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Forward mappings
        CreateMap<Entities.Location, LocationDto>();
        CreateMap<Entities.Address, AddressDto>();
        CreateMap<Entities.Prefecture, PrefectureDTO>();
        //Reverse mappings
        CreateMap<LocationDto, Entities.Location>();
        CreateMap<AddressDto, Entities.Address>();
        CreateMap<PrefectureDTO, Entities.Prefecture>();
    }
}