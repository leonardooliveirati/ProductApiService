using AutoMapper;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.DTOs;
using Product.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        await _productService.AddProductAsync(productDto);
        var productJson = System.Text.Json.JsonSerializer.Serialize(productDto);
        await _producer.ProduceAsync("Produtos", new Message<Null, string> { Value = productJson });
        return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
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

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
    {
        await _productService.UpdateProductAsync(productDto);
        _cache.Remove(productDto.Id); // Remove from cache to get updated data in future requests
        return NoContent();
    }

}
