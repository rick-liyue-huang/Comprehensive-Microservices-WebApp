using System.Linq.Expressions;
using ProductsService.DataAccessLayer.Entities;

namespace ProductsService.DataAccessLayer.RepositoryContracts;

public interface IProductsRepository
{
    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Product>> GetProducts();
    /// <summary>
    /// Get products by condition
    /// </summary>
    /// <param name="conditionExpression"></param>
    /// <returns></returns>
    Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);
    /// <summary>
    /// Get product by condition
    /// </summary>
    /// <param name="conditionExpression"></param>
    /// <returns></returns>
    Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);
    /// <summary>
    /// Add product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<Product?> AddProduct(Product product);
    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<Product?> UpdateProduct(Product product);
    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<bool> DeleteProduct(Guid productId);
}