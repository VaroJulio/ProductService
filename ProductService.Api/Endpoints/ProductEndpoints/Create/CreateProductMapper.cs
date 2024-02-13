using AutoMapper;
using ProductService.UseCases.Dtos;

namespace ProductService.Api.Endpoints.ProductEndpoints.Create
{
    public class CreateProductMapper : Profile
    {
        public CreateProductMapper() 
        {
            CreateMap<CreateProductRequest, CreateProductDto>();
            CreateMap<ProductDto, CreateProductResponse>();
        }
    }
}
