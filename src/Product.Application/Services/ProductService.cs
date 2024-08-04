using AutoMapper;
using Product.Application.DTOs;
using Product.Domain.Entities;
using Product.Domain.Interfaces;

namespace Product.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllProductsAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task AddProductAsync(ProductDto productDto)
    {
        var product = _mapper.Map<ProductEntity>(productDto);
        await _productRepository.AddProductAsync(product);
    }

    public async Task UpdateProductAsync(ProductDto productDto)
    {
        var product = _mapper.Map<ProductEntity>(productDto);
        await _productRepository.UpdateProductAsync(product);
    }
}
