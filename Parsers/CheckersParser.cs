using System.Globalization;
using HtmlAgilityPack;
using shopping_app.Parsers.Interface;
using shopping_app.Products;
using shopping_app.Products.Interface;

namespace shopping_app.Parsers;

public class CheckersParser : IParser
{
    public List<IProduct> GetProducts(HtmlDocument html, bool includeEmptyNames = false)
    {
        List<IProduct> products = [];

        var productCards = html.DocumentNode.Descendants("div")
            .Where(x => x.HasClass("item-product")).ToList();

        foreach (var productCard in productCards)
        {
            var itemDetails = productCard.Descendants("div")
                .FirstOrDefault(x => x.HasClass("item-product__details"));

            var name = itemDetails?.Descendants("h3")
                .FirstOrDefault(x => x.HasClass("item-product__name"))?.InnerText.Trim();
            if (string.IsNullOrWhiteSpace(name) && !includeEmptyNames) continue;

            var priceContainer = itemDetails?.Descendants("div")
                .FirstOrDefault(x => x.HasClass("special-price__price"));
            var priceText = priceContainer?.ChildNodes[1].InnerText.Trim();
            if (string.IsNullOrEmpty(priceText)) continue;

            // TODO: Get specials
            // var specialContainer = itemDetails?.Descendants("div")
            //     .FirstOrDefault(x => x.HasClass("special-price__extra"));

            var priceType = IProduct.Pricing.PerItem;
            double price;
            if (priceText.Contains("kg", StringComparison.CurrentCultureIgnoreCase))
            {
                price = double.Parse(priceText.TrimStart('R').Split(' ')[0].Trim(), CultureInfo.InvariantCulture);
                priceType = IProduct.Pricing.PerKg;
            }
            else
            {
                price = double.Parse(priceText.TrimStart('R').Trim(), CultureInfo.InvariantCulture);
            }
            
            var product = new CheckersProduct
            {
                Name = name,
                Price = price,
                PriceType = priceType
            };
            products.Add(product);
        }

        return products;
    }
}