using CartService.BLL.Interfaces;
using CartService.DAL.Entities;

namespace CartService.API.Endpoints;

public static class CartItemsEndpoints
{
    public static void AddCartEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var cartItemsRouteGroup = routeBuilder.MapGroup("api/cart/{cartId}/items");

        cartItemsRouteGroup
            .MapGet(string.Empty, async (int cartId, ICartService cartService) =>
            {
                var items = await cartService.GetItemsAsync(cartId);
                return Results.Ok(items);
            })
            .WithName("GetCartItems")
            .WithOpenApi();

        cartItemsRouteGroup
            .MapPost(string.Empty, async (int cartId, CartItem item, ICartService cartService) =>
            {
                await cartService.AddItemAsync(cartId, item);
                return Results.Ok();
            })
            .WithName("AddCartItem")
            .WithOpenApi();

        cartItemsRouteGroup
            .MapDelete("{itemId}", async (int cartId, int itemId, ICartService cartService) =>
            {
                await cartService.RemoveItemAsync(cartId, itemId);
                return Results.Ok();
            })
            .WithName("DeleteCartItem")
            .WithOpenApi();
    }
}
