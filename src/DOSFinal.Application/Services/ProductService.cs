using DOSFinal.Application.DTOs;
using DOSFinal.Application.Interfaces;
using DOSFinal.Domain.Entities;
using DOSFinal.Domain.Interfaces;

namespace DOSFinal.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
    {
        var products = await _productRepository.GetActiveProductsAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            Stock = createProductDto.Stock
        };

        var createdProduct = await _productRepository.AddAsync(product);
        return MapToDto(createdProduct);
    }

    public async Task UpdateAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new Exception("Product not found");

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;
        product.Stock = updateProductDto.Stock;
        product.IsActive = updateProductDto.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive
        };
    }
}
