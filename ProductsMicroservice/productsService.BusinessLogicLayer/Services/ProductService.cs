using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using productsService.BusinessLogicLayer.Dtos;
using productsService.BusinessLogicLayer.ServiceContracts;
using productsService.DataAccessLayer.Entities;
using productsService.DataAccessLayer.RepositoryContracts;

namespace productsService.BusinessLogicLayer.Services;

public class ProductService(
    IValidator<ProductAddRequest> productAddRequestValidator,
    IValidator<ProductUpdateRequest> productUpdateRequestValidator,
    IMapper mapper,
    IProductRepository productRepository
) : IProductService
{
    public async Task<List<ProductResponse>> GetProducts()
    {
        IEnumerable<Product?> products = await productRepository.GetProducts();
        IEnumerable<ProductResponse> productResponses = mapper.Map<IEnumerable<ProductResponse>>(products); // invoke ProductToProductResponseMappingProfile
        return productResponses.ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product?> products = await productRepository.GetProductsByCondition(conditionExpression);
        IEnumerable<ProductResponse?> productResponses = mapper.Map<IEnumerable<ProductResponse?>>(products);// invoke ProductToProductResponseMappingProfile
        return productResponses.ToList();
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await productRepository.GetProductByCondition(conditionExpression);
        if (product == null) return null;
        ProductResponse productResponse = mapper.Map<ProductResponse>(product); // invoke ProductToProductResponseMappingProfile
        return productResponse;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if (productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }
        
        ValidationResult result = await productAddRequestValidator.ValidateAsync(productAddRequest);
        
        if (!result.IsValid)
        {
            string errors = string.Join(", ", result.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        Product product = mapper.Map<Product>(productAddRequest);
        Product? addedProduct = await productRepository.AddProduct(product);
        if (addedProduct == null)
        {
            return null;
        }
        ProductResponse addedProductResponse = mapper.Map<ProductResponse>(addedProduct);
        return addedProductResponse;
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        Product? existingProduct = await productRepository.GetProductByCondition(temp => temp.ProductId == productUpdateRequest.ProductId);
        if (existingProduct == null)
        {
            throw new ArgumentException("Product not found");
        }
        
        ValidationResult result = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
        
        if (!result.IsValid)
        {
            string errors = string.Join(", ", result.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }
        
        Product product = mapper.Map<Product>(productUpdateRequest);
        Product? updatedProduct = await productRepository.UpdateProduct(product);
        if (updatedProduct == null)
        {
            return null;
        }
        ProductResponse updatedProductResponse = mapper.Map<ProductResponse>(updatedProduct);
        return updatedProductResponse;
        
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product? existingProduct = await productRepository.GetProductByCondition(temp => temp.ProductId == productId);
        if (existingProduct == null)
        {
            return false;
        }
        bool isDeleted = await productRepository.DeleteProduct(productId);
        return isDeleted;
        
    }
}