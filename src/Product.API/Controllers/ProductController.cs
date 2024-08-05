using AutoMapper;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.DTOs;
using Product.Application.Services;
using System.Text.Json;

namespace Product.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly IMemoryCache _cache;
    private readonly IProducer<Null, string> _producer;
    private readonly IMapper _mapper;

    public ProductController(ProductService productService, IMemoryCache cache, IProducer<Null, string> producer, IMapper mapper)
    {
        _productService = productService;
        _cache = cache;
        _producer = producer;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
    {
        try
        {
            await _productService.AddProductAsync(productDto);
            var productJson = JsonSerializer.Serialize(productDto);

            // Publica a mensagem no Kafka
            var deliveryReport = await _producer.ProduceAsync("Produtos", new Message<Null, string> { Value = productJson });

            if (deliveryReport.Status == PersistenceStatus.Persisted)
            {
                return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
            }
            else
            {
                // Handle error accordingly
                return StatusCode(500, "Failed to send message to Kafka");
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocorreu um erro interno no servidor. Tente novamente mais tarde." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            if (!_cache.TryGetValue(id, out ProductDto productDto))
            {
                productDto = await _productService.GetProductByIdAsync(id);
                if (productDto == null)
                {
                    return NotFound();
                }
                _cache.Set(id, productDto, TimeSpan.FromMinutes(5));
            }
            return Ok(productDto);

        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocorreu um erro interno no servidor. Tente novamente mais tarde." });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
    {
        try
        {
            await _productService.UpdateProductAsync(productDto);
            _cache.Remove(productDto.Id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocorreu um erro interno no servidor. Tente novamente mais tarde." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocorreu um erro interno no servidor. Tente novamente mais tarde." });
        }
    }
}
