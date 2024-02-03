using System.Globalization;
using HtmlAgilityPack;
using shopping_app.Parsers.Interface;
using shopping_app.Products;
using shopping_app.Products.Interface;

namespace shopping_app.Parsers;

public class WoolworthsParser : IParser
{
    public List<IProduct> GetProducts(HtmlDocument html, bool includeEmptyNames = false)
    {
        List<IProduct> products = [];

        var productCards = html.DocumentNode.Descendants("article").ToList().Where(x => x.HasClass("product-card"))
            .ToList();
        foreach (var productCard in productCards)
        {
            var details = productCard.Descendants()
                .FirstOrDefault(x => x.GetAttributeValue("id", "").Contains("prod_details"));
            var title = details?.Descendants("a")?.FirstOrDefault(x => x.HasClass("range--title"))?.InnerText.Trim();
            if (string.IsNullOrEmpty(title) && !includeEmptyNames) continue;

            var actions = productCard.Descendants()
                .FirstOrDefault(x => x.GetAttributeValue("id", "").Contains("prod_actions"));
            var priceText = actions?.Descendants().FirstOrDefault(x => x.HasClass("product__price"))
                ?.Descendants("strong").FirstOrDefault(x => x.HasClass("price"))?.InnerText;
            if (string.IsNullOrEmpty(priceText)) continue;

            var special = actions?.Descendants("div").FirstOrDefault(x => x.HasClass("product__special"))?.InnerText
                .Trim();

            var product = new WoolworthsProduct
            {
                Name = title,
                Price = double.Parse(priceText.TrimStart('R').Trim(), CultureInfo.InvariantCulture),
                Special = special,
                PriceType = IProduct.Pricing.PerItem
            };

            products.Add(product);
        }

        return products;
    }
}