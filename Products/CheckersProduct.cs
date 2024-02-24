using shopping_app.Products.Enums;
using shopping_app.Products.Interface;

namespace shopping_app.Products;

public class CheckersProduct : Product
{
    public CheckersProduct(string name, double price, string? special = null, PriceType? priceType = null)
    {
        Shop = Shop.Checkers;
        Name = name;
        Price = price;
        PriceType = priceType ?? Enums.PriceType.PerItem;
        Special = special;
    }
}