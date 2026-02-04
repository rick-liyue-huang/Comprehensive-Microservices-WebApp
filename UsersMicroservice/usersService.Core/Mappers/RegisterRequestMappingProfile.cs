using AutoMapper;
using usersService.Core.Dtos;
using usersService.Domain.Entities;

namespace usersService.Core.Mappers;

public class RegisterRequestMappingProfile : Profile
{
    public RegisterRequestMappingProfile()
    {
        CreateMap<RegisterRequest, ApplicationUser>()
            .ForMember(
                dest => dest.UserId,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Password, 
                opt => opt.MapFrom(src => src.Password))
            .ForMember(
                dest => dest.Email, 
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.PersonName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender));
    }
}