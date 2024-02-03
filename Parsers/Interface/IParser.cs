using HtmlAgilityPack;
using shopping_app.Products.Interface;

namespace shopping_app.Parsers.Interface;

public interface IParser
{
    List<IProduct> GetProducts(HtmlDocument html, bool includeEmptyNames = false);
}