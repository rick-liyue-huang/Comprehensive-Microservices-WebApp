using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using ProductsService.BusinessLogicLayer.Dtos;
using ProductsService.BusinessLogicLayer.ServiceContracts;
using ProductsService.DataAccessLayer.Entities;
using ProductsService.DataAccessLayer.RepositoryContracts;

namespace ProductsService.BusinessLogicLayer.Services;

public class ProductsService(
    IValidator<ProductAddRequest> productAddRequestValidator,
    IValidator<ProductUpdateRequest> productUpdateRequestValidator,
    IMapper mapper,
    IProductsRepository productsRepository
) : IProductsService
{
    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = await productsRepository.GetProducts();
        IEnumerable<ProductResponse?> productResponses = mapper.Map<IEnumerable<ProductResponse?>>(products);
        return productResponses.ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product?> products = await productsRepository.GetProductsByCondition(conditionExpression);
        IEnumerable<ProductResponse?> productResponses = mapper.Map<IEnumerable<ProductResponse?>>(products);
        return productResponses.ToList();
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await productsRepository.GetProductByCondition(conditionExpression);
        if (product == null)
        {
            return null;
        }
        ProductResponse productResponse = mapper.Map<ProductResponse>(product);
        return productResponse;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if (productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }
        
        // Validate the product using Fluent Validation
        ValidationResult result = await productAddRequestValidator.ValidateAsync(productAddRequest);
        
        // check the validation result
        if (!result.IsValid)
        {
            string errors = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }
        
        Product product = mapper.Map<Product>(productAddRequest);
        
        Product? addedProduct = await productsRepository.AddProduct(product);
        if (addedProduct == null)        {
            return null;
        }
        ProductResponse addedProductResponse = mapper.Map<ProductResponse>(addedProduct);
        return addedProductResponse;
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        Product? existingProduct = await productsRepository.GetProductByCondition(p => p.ProductId == productUpdateRequest.ProductId);
        if (existingProduct is null)
        {
            throw new ArgumentException("Invalid product ID");
        }
        
        // Validate the product using Fluent Validation
        ValidationResult result = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
        if (!result.IsValid)
        {
            string errors = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        };
        
        Product product = mapper.Map<Product>(productUpdateRequest);
        Product? updatedProduct = await productsRepository.UpdateProduct(product);
        if (updatedProduct == null)
        {
            return null;
        }
        ProductResponse updatedProductResponse = mapper.Map<ProductResponse>(updatedProduct);
        return updatedProductResponse;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product? product = await productsRepository.GetProductByCondition(p => p.ProductId == productId);

        if (product == null)
        {
            return false;
        }
        
        bool isDeleted = await productsRepository.DeleteProduct(productId);
        return isDeleted;
    }
}