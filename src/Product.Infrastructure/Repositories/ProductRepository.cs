using Microsoft.EntityFrameworkCore;
using Product.Domain.Interfaces;
using Product.Infrastructure.Data;
using Product.Domain.Entities;

namespace Product.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductContext _context;

    public ProductRepository(ProductContext context)
    {
        _context = context;
    }

    public async Task<ProductEntity> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task AddProductAsync(ProductEntity product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(ProductEntity product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
}
