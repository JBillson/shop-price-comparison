using HtmlAgilityPack;
using shopping_app.Parsers;
using shopping_app.Products;
using shopping_app.Products.Enums;
using shopping_app.Products.Interface;
using shopping_app.Utilities;

namespace shopping_app.Controllers;

public class ProductController
{
    private readonly WoolworthsParser _wooliesParser = new();
    private readonly PnpParser _pnpParser = new();
    private readonly CheckersParser _checkersParser = new();
    private readonly ProductCache _productCache = new();
    
    public async Task<List<Product>> GetProductsAsync(Shop shop)
    {
        List<Product> products;
        Console.WriteLine($"Getting {shop} Products");
        // return products from cache if not expired
        if (!_productCache.IsExpired(shop))
        {
            products = _productCache.GetProducts(shop);
            Console.WriteLine($"{products.Count} products returned from cache");
            return products;
        }

        Console.WriteLine($"Scraping {shop} Products");
        // scrape products from web
        var webPages = await RequestManager.ScrapeShop(shop);
        if (webPages == null || webPages.Count == 0)
        {
            Console.WriteLine("Failed to scrape products, returning cached products");
            return _productCache.GetProducts(shop);
        }

        // parse products
        products = ParseProducts(shop, webPages);
        Console.WriteLine($"{products.Count} Products scraped from {shop}");
        
        // update cache
        Console.WriteLine("Updating Cache");
        await _productCache.UpdateProducts(shop, products);
        
        return products;
    }

    private List<Product> ParseProducts(Shop shop, List<HtmlDocument> htmlDocs)
    {
        List<Product> products = [];
        switch (shop)
        {
            case Shop.Checkers:
                foreach (var htmlDoc in htmlDocs)
                    products.AddRange(_checkersParser.GetProducts(htmlDoc)); 
                break;
            case Shop.Woolworths:
                foreach (var htmlDoc in htmlDocs)
                    products.AddRange(_wooliesParser.GetProducts(htmlDoc)); 
                break;
            case Shop.PickNPay:
                foreach (var htmlDoc in htmlDocs)
                    products.AddRange(_pnpParser.GetProducts(htmlDoc)); 
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(shop), shop, null);
        }

        return products;
    }
}