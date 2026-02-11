using System.Xml;
using FluentValidation;
using FluentValidation.Results;
using MySqlX.XDevAPI.Common;
using productsService.BusinessLogicLayer.Dtos;
using productsService.BusinessLogicLayer.ServiceContracts;

namespace productsService.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        
        // add minimal api
        
        //GET /api/products
        app.MapGet("/api/products", async (IProductService productService) =>
        {
            List<ProductResponse?> products = await productService.GetProducts();
            if (products.Count == 0) return Results.NotFound();
            return Results.Ok(products);
            
        });

        app.MapGet(
            "/api/products/search/product-id/{ProductId:guid}",
            async (IProductService productsService, Guid productId) =>
        {
            ProductResponse? productResponse =
                await productsService.GetProductByCondition(temp => temp.ProductId == productId);
            if (productResponse == null) return Results.NotFound();
            return Results.Ok(productResponse);
        });

        app.MapGet("/api/products/search/{searchTerm}", async (IProductService productsService, string searchTerm) =>
        {
            List<ProductResponse?> productsByNameResponse = await productsService.GetProductsByCondition(
                temp => temp.ProductName != null && temp.ProductName.Contains(searchTerm));
    
            List<ProductResponse?> productsByCategoryResponse = await productsService.GetProductsByCondition(
                temp => temp.Category != null && temp.Category.Contains(searchTerm));
    
            var products = productsByNameResponse.Union(productsByCategoryResponse);
    
            if (!products.Any()) return Results.NotFound();
    
            return Results.Ok(products);
        });
        
        app.MapPost("/api/products", async (
            IProductService productsService, 
            ProductAddRequest productAddRequest,
            IValidator<ProductAddRequest> productAddRequestValidator) =>
        {
            ValidationResult result = await productAddRequestValidator.ValidateAsync(productAddRequest);
            if (!result.IsValid)
            {
                Dictionary<string, string[]> errors = result.Errors
                    .GroupBy(temp => temp.PropertyName)
                    .ToDictionary(
                        temp => temp.Key, 
                        temp => temp.Select(e => e.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }
            
            ProductResponse? productResponse = await productsService.AddProduct(productAddRequest);
            if (productResponse == null) return Results.Problem("Product could not be added");
            return Results.Created(@"/api/products/search/product-id/{productId}", productResponse);
        });


        app.MapPut("/api/products/", async (
            IProductService productsService, 
            ProductUpdateRequest productUpdateRequest,
            IValidator<ProductUpdateRequest> productUpdateRequestValidator) =>
        {
            ValidationResult result = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
            if (!result.IsValid)
            {
                Dictionary<string, string[]> errors = result.Errors
                    .GroupBy(temp => temp.PropertyName)
                    .ToDictionary(
                        temp => temp.Key, 
                        temp => temp.Select(e => e.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }
            
            ProductResponse? productResponse = await productsService.UpdateProduct(productUpdateRequest);
            if (productResponse == null) return Results.Problem("Product could not be updated");
            return Results.Ok(productResponse);
        });

        app.MapDelete("/api/products/{productId:guid}", async (
            IProductService productsService, Guid productId) =>
        {
            bool isDeleted = await productsService.DeleteProduct(productId);
            if (!isDeleted) return Results.Problem("Product could not be deleted");
            return Results.Ok(true);
            
        });
        
        return app;
    }
}