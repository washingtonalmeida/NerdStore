using AutoMapper;
using NerdStore.Catalog.Application.Dtos;
using NerdStore.Catalog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Catalog.Application.AutoMapper
{
    public class DtoToDomainMappingProfile : Profile
    {
        public DtoToDomainMappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ConstructUsing(dto => new Product(dto.Name, dto.Description, dto.Active, dto.Price, dto.CategoryId,
                                        dto.RegistrationDate, dto.Image, new Dimensions(dto.Height, dto.Width, dto.Depth)));

            CreateMap<CategoryDto, Category>()
                .ConstructUsing(dto => new Category(dto.Name, dto.Code));
        }
    }
}
