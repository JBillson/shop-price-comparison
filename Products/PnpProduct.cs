using shopping_app.Products.Enums;
using shopping_app.Products.Interface;

namespace shopping_app.Products;

public class PnpProduct : Product
{
    public PnpProduct(string name, double price, string? special = null, PriceType? priceType = null)
    {
        Shop = Shop.PickNPay;
        Name = name;
        Price = price;
        PriceType = priceType ?? Enums.PriceType.PerItem;
        Special = special;
    }
}