using FluentValidation;
using ProductsService.BusinessLogicLayer.Dtos;

namespace ProductsService.BusinessLogicLayer.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");
        
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required.");
        
        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid category.");
        
        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Unit price must be a non-negative value.");
        
        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity in stock must be a non-negative value.");
        
    }
}