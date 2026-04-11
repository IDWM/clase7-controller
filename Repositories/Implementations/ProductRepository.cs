using clase7_controller.Data;
using clase7_controller.Exceptions;
using clase7_controller.Models;
using clase7_controller.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace clase7_controller.Repositories.Implementations;

public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<Product>> GetPagedAsync(int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;
        IReadOnlyList<Product> products = await _dbContext
            .Products.AsNoTracking()
            .OrderBy(product => product.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        return products;
    }

    public async Task<int> CountAsync()
    {
        int total = await _dbContext.Products.CountAsync();
        return total;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        Product? product = await _dbContext
            .Products.AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id);
        return product;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected == 0)
        {
            throw new PersistenceException("Error al crear el producto.");
        }

        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected == 0)
        {
            throw new PersistenceException("Error al actualizar el producto.");
        }

        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await _dbContext.Products.FirstOrDefaultAsync(product =>
            product.Id == id
        );
        if (product is null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected == 0)
        {
            throw new PersistenceException("Error al eliminar el producto.");
        }

        return true;
    }
}
