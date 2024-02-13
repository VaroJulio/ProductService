using FastEndpoints;
using FluentValidation;

namespace ProductService.Api.Endpoints.ProductEndpoints.GetById
{
    public class GetByIdProductRequestValidator : Validator<GetByIdProductRequest>
    {
        public GetByIdProductRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).LessThan(int.MaxValue);
        }
    }
}
