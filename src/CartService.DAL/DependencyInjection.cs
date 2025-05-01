using System.Diagnostics.CodeAnalysis;
using CartService.DAL.Clients;
using CartService.DAL.Interfaces;
using CartService.DAL.Repositories;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace CartService.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration config)
    {
        string? connectionString = config.GetConnectionString("LiteDB");
        services.AddSingleton<ILiteDatabase, LiteDatabase>(p => new LiteDatabase(connectionString));

        services.AddScoped<ICartRepository, CartRepository>();

        string catalogServiceUrl = config.GetRequiredSection("CatalogServiceOptions:BaseUrl").Value!;
        services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>(client =>
            {
                client.BaseAddress = new Uri(catalogServiceUrl);
            })
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

        return services;
    }
}
