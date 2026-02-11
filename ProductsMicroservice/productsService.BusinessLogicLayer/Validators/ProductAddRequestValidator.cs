using FluentValidation;
using productsService.BusinessLogicLayer.Dtos;

namespace productsService.BusinessLogicLayer.Validators;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator()
    {
        RuleFor(x => x.ProductName).NotEmpty().WithMessage("ProductName is required.");
        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Category is invalid.");    
        RuleFor(x => x.UnitPrice)
            .InclusiveBetween(0, double.MaxValue).WithMessage("UnitPrice must be a non-negative value.");
        RuleFor(x => x.QuantityInStock).
            InclusiveBetween(0, int.MaxValue).WithMessage("QuantityInStock must be a non-negative value.");       
    }
}