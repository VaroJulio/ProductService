using AutoMapper;
using ProductService.UseCases.Dtos;

namespace ProductService.Api.Endpoints.ProductEndpoints.Update
{
    public class UpdateProductMapper : Profile
    {
        public UpdateProductMapper()
        {
            CreateMap<UpdateProductRequest, UpdateProductDto>();
            CreateMap<ProductDto, UpdateProductResponse>();
            CreateMap<ProductDto, UpdateProductRequest>();
        }
    }
}
