using DOSFinal.Application.DTOs;

namespace DOSFinal.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
    Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
    Task UpdateAsync(Guid id, UpdateProductDto updateProductDto);
    Task DeleteAsync(Guid id);
}
