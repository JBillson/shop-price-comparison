using System.Globalization;
using HtmlAgilityPack;
using shopping_app.Parsers.Interface;
using shopping_app.Products;
using shopping_app.Products.Enums;
using shopping_app.Products.Interface;

namespace shopping_app.Parsers;

public class CheckersParser : IParser
{
    public List<Product> GetProducts(HtmlDocument html)
    {
        List<Product> products = [];

        var productCards = html.DocumentNode.Descendants("div")
            .Where(x => x.HasClass("item-product")).ToList();

        foreach (var productCard in productCards)
        {
            // get name
            var itemDetails = productCard.Descendants("div")
                .FirstOrDefault(x => x.HasClass("item-product__details"));
            var name = itemDetails?.Descendants("h3")
                .FirstOrDefault(x => x.HasClass("item-product__name"))?.InnerText.Trim();
            if (string.IsNullOrWhiteSpace(name)) continue;

            // get price
            var priceContainer = itemDetails?.Descendants("div")
                .FirstOrDefault(x => x.HasClass("special-price__price"));
            var priceText = priceContainer?.ChildNodes[1].InnerText.Trim();
            if (string.IsNullOrEmpty(priceText)) continue;
            PriceType priceType;
            double price;
            if (priceText.Contains("kg", StringComparison.CurrentCultureIgnoreCase))
            {
                price = double.Parse(priceText.TrimStart('R').Split(' ')[0].Trim(), CultureInfo.InvariantCulture);
                priceType = PriceType.PerKg;
            }
            else
            {
                price = double.Parse(priceText.TrimStart('R').Trim(), CultureInfo.InvariantCulture);
                priceType = PriceType.PerItem;
            }

            // get special if it exists
            // var specialContainer = itemDetails?.Descendants("div")
            //     .FirstOrDefault(x => x.HasClass("special-price__extra"));
            var special = string.Empty;

            // create product
            var product = new CheckersProduct(name, price, special, priceType);
            products.Add(product);
        }

        return products;
    }
}