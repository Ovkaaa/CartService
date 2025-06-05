using System.Diagnostics.CodeAnalysis;
using CartService.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddBLL(this IServiceCollection services)
    {
        services.AddScoped<ICartItemService, BLL.Services.CartItemService>();

        return services;
    }
}
