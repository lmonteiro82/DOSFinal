using DOSFinal.Domain.Entities;
using DOSFinal.Domain.Interfaces;

namespace DOSFinal.Infrastructure.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    public Task<Product?> GetByIdAsync(Guid id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Product>>(_products);
    }

    public Task<Product> AddAsync(Product entity)
    {
        _products.Add(entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Product entity)
    {
        var index = _products.FindIndex(p => p.Id == entity.Id);
        if (index != -1)
        {
            _products[index] = entity;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _products.Remove(product);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        var activeProducts = _products.Where(p => p.IsActive);
        return Task.FromResult(activeProducts);
    }

    public Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var products = _products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        return Task.FromResult(products);
    }
}
