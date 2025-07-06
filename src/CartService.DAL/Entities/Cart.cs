using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace CartService.DAL.Entities;

[ExcludeFromCodeCoverage]
public class Cart
{
    public required int Id { get; set; }
    public Collection<CartItem> Items { get; } = [];
}
