using AutoMapper;
using productsService.BusinessLogicLayer.Dtos;
using productsService.DataAccessLayer.Entities;

namespace productsService.BusinessLogicLayer.Mappers;

public class ProductToProductResponseMappingProfile : Profile
{
    public ProductToProductResponseMappingProfile()
    {
        CreateMap<Product, ProductResponse>()
            .ForMember(
                dest => dest.Category, 
                opt => opt.MapFrom(src => src.Category))
            .ForMember(
                dest => dest.ProductId, 
                opt => opt.MapFrom(src => src.ProductId))
            .ForMember(
                dest => dest.ProductName, 
                opt => opt.MapFrom(src => src.ProductName))
            .ForMember(
                dest => dest.UnitPrice, 
                opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(
                dest => dest.QuantityInStock, 
                opt => opt.MapFrom(src => src.QuantityInStock));       
    }
}