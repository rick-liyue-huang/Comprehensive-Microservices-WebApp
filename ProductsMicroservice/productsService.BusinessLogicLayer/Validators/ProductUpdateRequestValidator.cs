using FluentValidation;
using productsService.BusinessLogicLayer.Dtos;

namespace productsService.BusinessLogicLayer.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.ProductName).NotEmpty().WithMessage("ProductName is required.");
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.").IsInEnum().WithMessage("Category is invalid.");    
        RuleFor(x => x.UnitPrice)
            .InclusiveBetween(0, double.MaxValue).WithMessage("UnitPrice must be a non-negative value.");
        RuleFor(x => x.QuantityInStock).
            InclusiveBetween(0, int.MaxValue).WithMessage("QuantityInStock must be a non-negative value.");       
    }
}