using System.Diagnostics.CodeAnalysis;

namespace CartService.DAL.Entities;

[ExcludeFromCodeCoverage]
public class CartItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? ImageAlt { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}