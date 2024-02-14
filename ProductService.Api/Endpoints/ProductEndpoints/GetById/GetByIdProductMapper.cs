using AutoMapper;
using ProductService.UseCases.Dtos;

namespace ProductService.Api.Endpoints.ProductEndpoints.GetById
{
    public class GetByIdProductMapper : Profile
    {

        public GetByIdProductMapper()
        {
            CreateMap<DetailedProductDto?, GetByIdProductResponse?>();
        }
    }
}
