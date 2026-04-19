using System.Linq.Expressions;
using ProductsService.BusinessLogicLayer.Dtos;
using ProductsService.DataAccessLayer.Entities;

namespace ProductsService.BusinessLogicLayer.ServiceContracts;

public interface IProductsService
{
        /// <summary>
        /// Get all products response
        /// </summary>
        /// <returns></returns>
        Task<List<ProductResponse?>> GetProducts();
        /// <summary>
        /// Get products response by condition
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);
        /// <summary>
        /// Get a product response by condition
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);
        /// <summary>
        /// Add product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProductResponse?> AddProduct(ProductAddRequest product);
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProductResponse?> UpdateProduct(ProductUpdateRequest product);
        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> DeleteProduct(Guid productId);
}