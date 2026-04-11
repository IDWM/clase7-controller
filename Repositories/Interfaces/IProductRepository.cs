using clase7_controller.Models;

namespace clase7_controller.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetPagedAsync(int page, int pageSize);
    Task<int> CountAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
}
