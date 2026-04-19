using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductsService.DataAccessLayer.Context;
using ProductsService.DataAccessLayer.Entities;
using ProductsService.DataAccessLayer.RepositoryContracts;

namespace ProductsService.DataAccessLayer.Repositories;

public class ProductsRepository(ProductDbContext context) : IProductsRepository
{
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await context.Products.Where(conditionExpression).ToListAsync();
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await context.Products.FirstOrDefaultAsync(conditionExpression);
    }

    public async Task<Product?> AddProduct(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        Product? existingProduct = await context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
        
        if (existingProduct is not null)
        {
            existingProduct.ProductName = product.ProductName;
            existingProduct.Category = product.Category;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.QuantityInStock = product.QuantityInStock;

            await context.SaveChangesAsync();
            return existingProduct;
        }
        else
        {
            return null;
        }
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product? existingProduct = await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        
        if (existingProduct is not null)
        {
            context.Products.Remove(existingProduct);
            int affectedRow = await context.SaveChangesAsync();
            return affectedRow > 0;
        }
        else
        {
            return false;
        }

    }
}