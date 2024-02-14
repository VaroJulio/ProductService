using FastEndpoints;
using FluentValidation;

namespace ProductService.Api.Endpoints.ProductEndpoints.Update
{
    public class UpdateProductRequestValidator : Validator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).LessThan(int.MaxValue);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).LessThanOrEqualTo(int.MaxValue);
            RuleFor(x => x.Price).PrecisionScale(18, 2, true);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        }
    }
}
