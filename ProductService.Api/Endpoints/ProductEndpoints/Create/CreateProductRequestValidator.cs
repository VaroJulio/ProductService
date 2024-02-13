using FastEndpoints;
using FluentValidation;

namespace ProductService.Api.Endpoints.ProductEndpoints.Create
{
    public class CreateProductRequestValidator : Validator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).LessThanOrEqualTo(int.MaxValue);
            RuleFor(x => x.Price).PrecisionScale(18, 2, true);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        }
    }
}
