namespace productsService.BusinessLogicLayer.Dtos;

public record ProductResponse(
    Guid ProductId,
    string ProductName,
    CategoryOptions Category,
    double? UnitPrice,
    int? QuantityInStock
)
{
    public ProductResponse() : this(Guid.Empty, "", default, null, null) { }
}