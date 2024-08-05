using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Product.API;
using Product.Application.Services;
using Product.Domain.Interfaces;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repositories;
using System.Reflection;


namespace Pdoduct.API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<ProductContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Produto API", Version = "v1" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ProductService>();
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddMemoryCache();

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = builder.Configuration["Kafka:BootstrapServers"]
        };

        builder.Services.AddSingleton<IProducer<Null, string>>(provider =>
            new ProducerBuilder<Null, string>(producerConfig).Build());

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Boleto API V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}