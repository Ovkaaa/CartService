using CartService.API.Endpoints;

namespace CartService.API.Extensions;

public static class WebApplicationExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var baseApiRouteGroup = app.MapGroup("api");
        var versionRouteGroup = baseApiRouteGroup.MapGroup("v1");
        versionRouteGroup.AddCartEndpoints();
    }
}
