using Newtonsoft.Json;
using shopping_app.Products.Enums;
using shopping_app.Products.Interface;

namespace shopping_app.Products;

public class ProductCache : IProductCache
{
    private readonly string _cachePath;
    private List<Product> _cachedProducts = [];
    private DateTime _lastUpdated;
    private bool _hasInitialised;

    public ProductCache()
    {
        _cachePath = $"{AppDomain.CurrentDomain.BaseDirectory}/../../../data/ProductCache.json";
        InitCache();
    }

    private async void InitCache()
    {
        _lastUpdated = DateTime.Now;
        try
        {
            var raw = await File.ReadAllTextAsync(_cachePath);
            _cachedProducts = JsonConvert.DeserializeObject<List<Product>>(raw) ?? [];
        }
        catch (Exception e)
        {
            switch (e)
            {
                case FileNotFoundException:
                    Console.WriteLine("No cache file found");
                    _cachedProducts = [];
                    break;
                default:
                    Console.WriteLine(e);
                    throw;
            }
        }
        _hasInitialised = true;
    }

    public List<Product> GetProducts(Shop shop)
    {
        while (!_hasInitialised)
        {
        }
        
        return _cachedProducts.Where(x => x.Shop == shop).ToList();
    }

    public async Task UpdateProducts(Shop shop, List<Product> products)
    {
        try
        {
            _cachedProducts.RemoveAll(x => x.Shop == shop);
            _cachedProducts.AddRange(products);
            await File.WriteAllTextAsync(_cachePath, JsonConvert.SerializeObject(_cachedProducts, Formatting.Indented));
            _lastUpdated = DateTime.Now;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to save products!\n" +
                              $"{e.Message}");
            throw;
        }
    }

    public bool IsExpired(Shop shop)
    {
        while (!_hasInitialised)
        {
        }

        var shopProducts = _cachedProducts.Where(x => x.Shop == shop).ToList();
        return _lastUpdated.AddDays(1) < DateTime.Now
               || shopProducts.Count == 0;
    }
}