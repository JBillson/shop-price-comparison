using shopping_app.Products.Enums;
using shopping_app.Products.Interface;

namespace shopping_app.Products;

public class WoolworthsProduct : Product
{
    public WoolworthsProduct(string name, double price, string? special = null, PriceType? priceType = null)
    {
        Shop = Shop.Woolworths;
        Name = name;
        Price = price;
        PriceType = priceType ?? Enums.PriceType.PerItem;
        Special = special;
    }
}