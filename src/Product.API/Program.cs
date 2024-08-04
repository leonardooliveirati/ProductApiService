using AutoMapper;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Product.API;
using Product.Application.Services;
using Product.Domain.Interfaces;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMemoryCache();

var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
