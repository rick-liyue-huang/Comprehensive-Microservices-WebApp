namespace ProductsService.BusinessLogicLayer.Dtos;

public record ProductAddRequest(
    string ProductName,
    CategoryOptions Category,
    double? UnitPrice,
    int? QuantityInStock
)
{
    public ProductAddRequest(): this("", CategoryOptions.Electronics, null, null) { }
}