using System.Globalization;
using HtmlAgilityPack;
using shopping_app.Parsers.Interface;
using shopping_app.Products;
using shopping_app.Products.Interface;

namespace shopping_app.Parsers;

public class WoolworthsParser : IParser
{
    public List<Product> GetProducts(HtmlDocument html)
    {
        List<Product> products = [];

        var productCards = html.DocumentNode.Descendants("article").ToList()
            .Where(x => x.HasClass("product-card")).ToList();
        foreach (var productCard in productCards)
        {
            // get name
            var details = productCard.Descendants()
                .FirstOrDefault(x => x.GetAttributeValue("id", "").Contains("prod_details"));
            var name = details?.Descendants("a")?.FirstOrDefault(x => x.HasClass("range--title"))?.InnerText.Trim();
            if (string.IsNullOrEmpty(name)) continue;

            // get price
            var actions = productCard.Descendants()
                .FirstOrDefault(x => x.GetAttributeValue("id", "").Contains("prod_actions"));
            var priceText = actions?.Descendants().FirstOrDefault(x => x.HasClass("product__price"))
                ?.Descendants("strong").FirstOrDefault(x => x.HasClass("price"))?.InnerText;
            if (string.IsNullOrEmpty(priceText)) continue;
            var price = double.Parse(priceText.TrimStart('R').Trim(), CultureInfo.InvariantCulture);

            // get special if it exists
            var special = actions?.Descendants("div").FirstOrDefault(x => x.HasClass("product__special"))?.InnerText.Trim();

            // create product
            var product = new WoolworthsProduct(name, price, special);
            products.Add(product);
        }

        return products;
    }
}