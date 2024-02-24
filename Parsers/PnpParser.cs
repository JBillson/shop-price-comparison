using System.Globalization;
using HtmlAgilityPack;
using shopping_app.Parsers.Interface;
using shopping_app.Products;
using shopping_app.Products.Interface;

namespace shopping_app.Parsers;

public class PnpParser : IParser
{
    public List<Product> GetProducts(HtmlDocument html)
    {
        List<Product> products = [];

        var itemGrid = html.DocumentNode.Descendants("div").ToList()
            .FirstOrDefault(x => x.HasClass("cx-product-container--grid"));
        var items = itemGrid?.Descendants("div").Where(x => x.HasClass("product-grid-item")).ToList();
        if (items == null) return products;

        foreach (var item in items)
        {
            // get name
            var infoContainer = item.FirstChild.Descendants("div")
                .FirstOrDefault(x => x.HasClass("product-grid-item__info-container"));
            var name = infoContainer?.Descendants("a")
                .FirstOrDefault(x => x.HasClass("product-grid-item__info-container__name"))?.InnerText.Trim();
            if (string.IsNullOrWhiteSpace(name)) continue;

            // get price
            var priceText = infoContainer?.Descendants("div")
                .FirstOrDefault(x => x.HasClass("product-grid-item__price-container"))?.InnerText.Trim();
            if (string.IsNullOrWhiteSpace(priceText)) continue;
            var price = double.Parse(priceText.TrimStart('R').Trim(), CultureInfo.InvariantCulture);

            // get special
            var special = infoContainer?.Descendants("div")
                .FirstOrDefault(x => x.HasClass("product-grid-item__promotion-container"))?.InnerText.Trim();

            // create product
            var product = new PnpProduct(name, price, special);
            products.Add(product);
        }

        return products;
    }
}