using AutoMapper;
using Moq;
using Product.Application.DTOs;
using Product.Application.Services;
using Product.Domain.Entities;
using Product.Domain.Interfaces;
public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductEntity, ProductDto>().ReverseMap());
        _mapper = config.CreateMapper();
        _productService = new ProductService(_productRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task AddProductAsync_Should_Call_AddProductAsync_Once()
    {
        var productDto = new ProductDto { Id = 1, Name = "Test Product", Price = 100, Description = "Test Description" };
        await _productService.AddProductAsync(productDto);
        _productRepositoryMock.Verify(repo => repo.AddProductAsync(It.IsAny<ProductEntity>()), Times.Once);
    }
}
