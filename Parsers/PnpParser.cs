using System.Globalization;
using HtmlAgilityPack;
using shopping_app.Parsers.Interface;
using shopping_app.Products;
using shopping_app.Products.Interface;

namespace shopping_app.Parsers;

public class PnpParser : IParser
{
    public List<IProduct> GetProducts(HtmlDocument html, bool includeEmptyNames = false)
    {
        List<IProduct> products = [];

        var itemGrid = html.DocumentNode.Descendants("div").ToList()
            .FirstOrDefault(x => x.HasClass("cx-product-container--grid"));
        var items = itemGrid?.Descendants("div").Where(x => x.HasClass("product-grid-item")).ToList();
        if (items == null) return products;

        foreach (var item in items)
        {
            var infoContainer = item.FirstChild.Descendants("div")
                .FirstOrDefault(x => x.HasClass("product-grid-item__info-container"));

            var title = infoContainer?.Descendants("a")
                .FirstOrDefault(x => x.HasClass("product-grid-item__info-container__name"))?.InnerText.Trim();
            if (string.IsNullOrWhiteSpace(title) && !includeEmptyNames) continue;

            var price = infoContainer?.Descendants("div")
                .FirstOrDefault(x => x.HasClass("product-grid-item__price-container"))?.InnerText.Trim();
            if (string.IsNullOrWhiteSpace(price)) continue;

            var special = infoContainer?.Descendants("div")
                .FirstOrDefault(x => x.HasClass("product-grid-item__promotion-container"))?.InnerText.Trim();

            var product = new PnpProduct
            {
                Name = title,
                Price = double.Parse(price.TrimStart('R').Trim(), CultureInfo.InvariantCulture),
                Special = special,
                PriceType = IProduct.Pricing.PerItem
            };
            products.Add(product);
        }

        return products;
    }
}