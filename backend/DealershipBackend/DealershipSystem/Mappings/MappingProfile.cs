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
        //Reverse mappings
        CreateMap<LocationDto, Location>();
        CreateMap<AddressDto, Address>();
        CreateMap<PrefectureDTO, Prefecture>();
        CreateMap<UserDTO, User>();
    }
}