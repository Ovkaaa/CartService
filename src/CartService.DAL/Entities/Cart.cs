using System.Diagnostics.CodeAnalysis;

namespace CartService.DAL.Entities;

[ExcludeFromCodeCoverage]
public class Cart
{
    public int Id { get; set; }
    public List<CartItem> Items { get; set; } = [];
}
