using FastEndpoints;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;

namespace ProductService.Api.Endpoints.ProductEndpoints.Create
{
    public class Create : Endpoint<CreateProductRequest, CreateProductResponse>
    {
        private readonly IProductService productService;
        private readonly AutoMapper.IMapper createProductMapper;

        public Create(IProductService productService, AutoMapper.IMapper createProductMapper)
        {
            this.productService = productService;
            this.createProductMapper = createProductMapper;
        }

        public override void Configure()
        {
            Post(CreateProductRequest.Route);
            Options(x => x.WithTags("ProductEndpoints"));
        }

        public override async Task HandleAsync(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var createProductDto = createProductMapper.Map<CreateProductDto>(request);
            var result = await productService.CreateProductAsync(createProductDto, cancellationToken);
            var createProductResponse = createProductMapper.Map<CreateProductResponse>(result);
            await SendAsync(createProductResponse, StatusCodes.Status201Created, cancellationToken);
        }
    }
}
