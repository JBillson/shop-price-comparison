using HtmlAgilityPack;
using shopping_app.Parsers;
using shopping_app.Products;
using shopping_app.Products.Enums;
using shopping_app.Products.Interface;
using shopping_app.Utilities;

namespace shopping_app.Controllers;

public class ProductController
{
    
    private static readonly WoolworthsParser WooliesParser = new();
    private static readonly PnpParser PnpParser = new();
    private static readonly CheckersParser CheckersParser = new();
    
    
    public static async Task<List<IProduct>> GetProductsAsync(Shop shop, bool searchMultiplePages = false)
    {
        var webPages = await RequestManager.ScrapeShop(shop, searchMultiplePages);
        if (webPages == null || webPages.Count == 0) return [];

        var products = ParseProducts(webPages);
        await ProductCache.UpdateProducts(shop, products);
        
        return products;
    }

    public static List<IProduct> ParseProducts(List<HtmlDocument> htmlDocuments)
    {
        
    }
}