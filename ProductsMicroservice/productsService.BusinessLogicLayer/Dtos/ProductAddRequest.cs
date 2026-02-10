namespace productsService.BusinessLogicLayer.Dtos;

public record ProductAddRequest(
    string ProductName,
    CategoryOptions Category,
    double? UnitPrice,
    int? QuantityInStock
)
{
    public ProductAddRequest() : this("", default, null, null) { }
}