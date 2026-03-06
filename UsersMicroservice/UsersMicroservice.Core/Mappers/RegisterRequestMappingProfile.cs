using AutoMapper;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Domain.Entities;

namespace UsersMicroservice.Core.Mappers;

public class RegisterRequestMappingProfile : Profile
{
    public RegisterRequestMappingProfile()
    {
        CreateMap<RegisterRequest, ApplicationUser>()
            .ForMember(
                dest => dest.Email,
                opt
                    => opt.MapFrom(src => src.Email))
            .ForMember(
                dest => dest.Password,
                opt
                    => opt.MapFrom(src => src.Password))
            .ForMember(
                dest => dest.PersonName,
                opt
                    => opt.MapFrom(src => src.PersonName))
            .ForMember(
                dest => dest.Gender,
                opt => opt.MapFrom(src => src.Gender))
            .ForMember(
                dest => dest.UserId,
                opt => opt.Ignore());

    }
}