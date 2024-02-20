using System.Collections;
using Newtonsoft.Json;
using shopping_app.Products.Enums;
using shopping_app.Products.Interface;

namespace shopping_app.Products;

public class ProductCache : IProductCache
{
    private const string Path = @"C:\Users\justi\Documents\work\personal\shopping-app\data\productCache.json";
    private List<IProduct> _cachedProducts = [];

    public async Task InitCache()
    {
        var raw = await File.ReadAllTextAsync(Path);
        _cachedProducts = JsonConvert.DeserializeObject<List<IProduct>>(raw) ?? [];
    }
    
    public async Task UpdateProducts(Shop shop, List<IProduct> products)
    {
        try
        {
            _cachedProducts.RemoveAll(x => x.Shop == shop);
            _cachedProducts.AddRange(products);
            await File.WriteAllTextAsync(Path, JsonConvert.SerializeObject(products));
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to save products!\n" +
                              $"{e.Message}");
            throw;
        }
    }
}