namespace ProductsService.BusinessLogicLayer.Dtos;

public record ProductUpdateRequest(
    Guid ProductId,
    string ProductName,
    CategoryOptions Category,
    double? UnitPrice,
    int? QuantityInStock)
{
    public ProductUpdateRequest(): this(Guid.Empty, "", CategoryOptions.Electronics, null, null) { }
}