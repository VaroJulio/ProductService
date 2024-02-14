using FastEndpoints;
using ProductService.UseCases.Interfaces;

namespace ProductService.Api.Endpoints.ProductEndpoints.GetById
{
    public class GetById : Endpoint<GetByIdProductRequest, GetByIdProductResponse?>
    {
        private readonly IProductService productService;
        private readonly AutoMapper.IMapper getByIdProductMapper;

        public GetById(IProductService productService, AutoMapper.IMapper getByIdProductMapper)
        {
            this.productService = productService;
            this.getByIdProductMapper = getByIdProductMapper;
        }

        public override void Configure()
        {
            Get(GetByIdProductRequest.Route);
            Options(x => x.WithTags("ProductEndpoints"));
        }

        public override async Task HandleAsync(GetByIdProductRequest request, CancellationToken cancellationToken)
        {
            var result = await productService.GetProductByIdAsync(request.Id, cancellationToken);     
            if (result is null) { await SendNotFoundAsync(cancellationToken); return; }
            var getByIdProductResponse = getByIdProductMapper.Map<GetByIdProductResponse?>(result);
            await SendOkAsync(getByIdProductResponse, cancellationToken);
        }
    }
}
