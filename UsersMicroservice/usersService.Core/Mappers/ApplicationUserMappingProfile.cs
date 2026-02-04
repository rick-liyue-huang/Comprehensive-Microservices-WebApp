using AutoMapper;
using usersService.Core.Dtos;
using usersService.Domain.Entities;

namespace usersService.Core.Mappers;

public class ApplicationUserMappingProfile : Profile
{
    public ApplicationUserMappingProfile()
    {
        CreateMap<ApplicationUser, AuthenticationResponse>()
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
            .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(
                dest => dest.PersonName,
                opt => opt.MapFrom(src => src.PersonName))
            .ForMember(
                dest => dest.Gender,
                opt => opt.MapFrom(src => src.Gender))
            .ForMember(
                dest => dest.Token,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Success,
                opt => opt.Ignore());
    }
}