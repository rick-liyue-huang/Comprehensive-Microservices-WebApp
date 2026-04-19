using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductsService.BusinessLogicLayer.Dtos;
using ProductsService.BusinessLogicLayer.ServiceContracts;

namespace ProductsService.API.ApiEndpoints;

public static class ProductApiEndpoints
{
    public static IEndpointRouteBuilder MapProductApiEndpoints(this IEndpointRouteBuilder app)
    {
        // GET: /api/products
        app.MapGet("/api/products", async (IProductsService productService) =>
        {
            List<ProductResponse?> products = await productService.GetProducts();
            return products.Count > 0 ? Results.Ok(products) : Results.NotFound();
        });
        
        // GET: /api/products/search/{id}
        app.MapGet("/api/products/search/product-id/{productId:guid}", 
            async (IProductsService productService, Guid productId) =>
        {
            ProductResponse? product = await productService.GetProductByCondition(p => p.ProductId == productId);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        });
        
        // GET: /api/products/search/{searchString}
        app.MapGet("/api/products/search/{searchString}",
            async (IProductsService productService, string searchString) =>
        {
            List<ProductResponse?> productsByName = await productService.GetProductsByCondition(p =>
                p.ProductName != null && EF.Functions.Like(p.ProductName, $"%{searchString}%"));

            List<ProductResponse?> productsByCategory = await productService.GetProductsByCondition(p =>
                p.Category != null && EF.Functions.Like(p.Category, $"%{searchString}%"));

            List<ProductResponse?> products = productsByName.Union(productsByCategory).ToList();

            return products.Count > 0 ? Results.Ok(products) : Results.NotFound();
        });
        
        // POST: /api/products
        app.MapPost("/api/products", 
            async (IProductsService productService, IValidator<ProductAddRequest> addedValidator, ProductAddRequest request) =>
        {
            var validationResult = await addedValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(tempe => tempe.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }
            
            ProductResponse? addedProduct = await productService.AddProduct(request);
            return addedProduct is not null ? 
                Results.Created($"/api/products/search/product-id/{addedProduct.ProductId}", addedProduct) : 
                Results.Problem("Failed to add product.");
        });
        
        // PUT: /api/products
        app.MapPut("/api/products", 
            async (IProductsService productService, IValidator<ProductUpdateRequest> updateValidator, ProductUpdateRequest request) =>
            {
                var validationResult = await updateValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(tempe => tempe.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }
            
                ProductResponse? updatedProduct = await productService.UpdateProduct(request);
                return updatedProduct is not null ? 
                    Results.Created($"/api/products/search/product-id/{updatedProduct.ProductId}", updatedProduct) : 
                    Results.Problem("Failed to update product.");
            });
        
        // DELETE: /api/products/{id}
        app.MapDelete("/api/products/{productId:guid}", async (IProductsService productsService, Guid productId) =>
        {
            bool isDeleted = await productsService.DeleteProduct(productId);
            return isDeleted ? Results.Ok(true) : Results.Problem("Failed to delete product.");
        });
        
        return app;
    }
}