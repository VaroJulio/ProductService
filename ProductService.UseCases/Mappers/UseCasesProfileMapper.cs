using AutoMapper;
using ProductService.Domain.ProductAggregate;
using ProductService.UseCases.Dtos;

namespace ProductService.UseCases.Mappers
{
    public class UseCasesProfileMapper  : Profile
    {
        public UseCasesProfileMapper() 
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<CreateProductDto, ProductDto>();
        }
    }
}
