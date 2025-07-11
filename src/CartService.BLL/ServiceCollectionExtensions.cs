using CartService.BLL.IntegrationEvents.Handlers;
using CartService.BLL.Interfaces;
using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CartService.BLL;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions

{
    public static IServiceCollection AddBLL(this IServiceCollection services)
    {
        services.AddSingleton<ICartItemService, CartItemService>();
        services.AddSingleton<IProductService, ProductService>();

        services.AddSingleton<IProductCreatedHandler, ProductCreatedHandler>();
        services.AddSingleton<IProductUpdatedHandler, ProductUpdatedHandler>();

        return services;
    }
}
