using CartService.DAL.Interfaces;
using CartService.DAL.Repositories;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CartService.DAL;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration config)
    {
        string? connectionString = config.GetConnectionString("LiteDB");
        services.AddSingleton<ILiteDatabase, LiteDatabase>(p => new LiteDatabase(connectionString));

        services.AddSingleton<ICartRepository, CartRepository>();
        services.AddSingleton<IProductRepository, ProductRepository>();

        return services;
    }
}
