using AutoMapper;
using Product.Application.DTOs;
using Product.Domain.Entities;

namespace Product.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductEntity, ProductDto>().ReverseMap();
    }
}
