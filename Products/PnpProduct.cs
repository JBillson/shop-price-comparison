using shopping_app.Products.Interface;

namespace shopping_app.Products;

public class PnpProduct : IProduct
{
    public required string? Name { get; set; }
    public required double Price { get; set; }
    public IProduct.Pricing? PriceType { get; set; }
    public string? Special { get; set; }
}