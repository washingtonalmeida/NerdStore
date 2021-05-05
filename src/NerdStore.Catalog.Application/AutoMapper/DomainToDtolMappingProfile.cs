using AutoMapper;
using NerdStore.Catalog.Application.Dtos;
using NerdStore.Catalog.Domain.Models;

namespace NerdStore.Catalog.Application.AutoMapper
{
    public class DomainToDtolMappingProfile : Profile
    {
        public DomainToDtolMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.Height, x => x.MapFrom(domain => domain.Dimensions.Height))
                .ForMember(dto => dto.Width, x => x.MapFrom(domain => domain.Dimensions.Width))
                .ForMember(dto => dto.Depth, x => x.MapFrom(domain => domain.Dimensions.Depth));

            CreateMap<Category, CategoryDto>();
        }
    }
}
