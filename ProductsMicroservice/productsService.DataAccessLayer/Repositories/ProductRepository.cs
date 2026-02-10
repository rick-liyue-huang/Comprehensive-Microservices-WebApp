using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using productsService.DataAccessLayer.Context;
using productsService.DataAccessLayer.Entities;
using productsService.DataAccessLayer.RepositoryContracts;

namespace productsService.DataAccessLayer.Repositories;

public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await dbContext.Products.Where(conditionExpression).ToListAsync();
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await dbContext.Products.FirstOrDefaultAsync(conditionExpression);
    }

    public async Task<Product?> AddProduct(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        Product? existingProduct = await dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductId == product.ProductId);
        
        if (existingProduct == null)
        {
            return null;
        }
        
        existingProduct.ProductName = product.ProductName;
        existingProduct.Category = product.Category;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.QuantityInStock = product.QuantityInStock;
        
        await dbContext.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product? existingProduct = await dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductId == productId);
        
        if (existingProduct == null)
        {
            return false;
        }
        
        dbContext.Products.Remove(existingProduct);
        int affectedRowsCount = await dbContext.SaveChangesAsync();
        return affectedRowsCount > 0;
    }
}