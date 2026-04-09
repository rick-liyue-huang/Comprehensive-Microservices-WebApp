
using AutoMapper;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Domain.Entities;

namespace UsersMicroservice.Core.Mappers;

public class ApplicationUserMappingProfile : Profile
{
  public ApplicationUserMappingProfile()
  {
    CreateMap<ApplicationUser, AuthenticationResponse>()
      .ForMember(
        destination => destination.UserId,
        opt => opt.MapFrom(source => source.UserId))
      .ForMember(
        destination => destination.Email,
        opt => opt.MapFrom(source => source.Email))
      .ForMember(
        destination => destination.PersonName,
        opt => opt.MapFrom(source => source.PersonName))
      .ForMember(
        destination => destination.Gender,
        opt => opt.MapFrom(source => source.Gender))
      .ForMember(
        destination => destination.Token,
        opt => opt.Ignore())
      .ForMember(
        destination => destination.Success,
        opt => opt.Ignore());
  }
}
