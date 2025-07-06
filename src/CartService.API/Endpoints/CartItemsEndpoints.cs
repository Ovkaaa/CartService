using CartService.API.Auth;
using CartService.BLL.Interfaces;
using CartService.DAL.Entities;

namespace CartService.API.Endpoints;

public static class CartItemsEndpoints
{
    public static void AddCartEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var cartItemsRouteGroup = routeBuilder.MapGroup("carts/{cartId}/items");

        cartItemsRouteGroup
            .MapGet(string.Empty, async (int cartId, ICartItemService cartItemService, CancellationToken cancellationToken) =>
            {
                var items = await cartItemService.GetCartItemsAsync(cartId, cancellationToken);
                return Results.Ok(items);
            })
            .WithName("GetCartItems")
            .RequireAuthorization(AuthPolicy.CanRead)
            .WithOpenApi();

        cartItemsRouteGroup
            .MapPost(string.Empty, async (int cartId, CartItem item, ICartItemService cartItemService, CancellationToken cancellationToken) =>
            {
                await cartItemService.AddCartItemAsync(cartId, item, cancellationToken);
                return Results.Ok();
            })
            .WithName("AddCartItem")
            .RequireAuthorization(AuthPolicy.CanCreate)
            .WithOpenApi();

        cartItemsRouteGroup
            .MapDelete("{itemId}", async (int cartId, int itemId, ICartItemService cartItemService, CancellationToken cancellationToken) =>
            {
                await cartItemService.RemoveCartItemAsync(cartId, itemId, cancellationToken);
                return Results.Ok();
            })
            .WithName("DeleteCartItem")
            .RequireAuthorization(AuthPolicy.CanDelete)
            .WithOpenApi();
    }
}
