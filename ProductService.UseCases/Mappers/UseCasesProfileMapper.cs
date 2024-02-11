using AutoMapper;
using ProductService.Domain.ProductAggregate;
using ProductService.UseCases.Dtos;

namespace ProductService.UseCases.Mappers
{
    public class UseCasesProfileMapper  : Profile
    {
        public UseCasesProfileMapper() 
        {
            CreateMap<Product?, ProductDto?>().ReverseMap();
            CreateMap<CreateProductDto, Product>();
            CreateMap<CreateProductDto, ProductDto>();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product?, DetailedProductDto?>();
        }
    }
}
