using FastEndpoints;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;

namespace ProductService.Api.Endpoints.ProductEndpoints.Update
{
    public class Update : Endpoint<UpdateProductRequest, UpdateProductResponse?>
    {
        private readonly IProductService productService;
        private readonly AutoMapper.IMapper updateProductMapper;

        public Update(IProductService productService, AutoMapper.IMapper updateProductMapper)
        {
            this.productService = productService;
            this.updateProductMapper = updateProductMapper;
        }

        public override void Configure()
        {
            Patch(UpdateProductRequest.Route);
            Options(x => x.WithTags("ProductEndpoints"));
        }

        public override async Task HandleAsync(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var updateProductDto = updateProductMapper.Map<UpdateProductDto>(request);
            var result = await productService.UpdateProductAsync(updateProductDto, cancellationToken);
            if (result is null) { await SendNotFoundAsync(cancellationToken); return; }
            var updateProductResponse = updateProductMapper.Map<UpdateProductResponse>(result);
            await SendOkAsync(updateProductResponse, cancellationToken);
        }
    }
}
