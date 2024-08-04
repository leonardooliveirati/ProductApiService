using Product.Domain.Entities;

namespace Product.Domain.Interfaces;

public interface IProductRepository
{
    Task<ProductEntity> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
    Task AddProductAsync(ProductEntity product);
    Task UpdateProductAsync(ProductEntity product);
}
