using Newtonsoft.Json;
using shopping_app.Parsers;
using shopping_app.Products;
using shopping_app.Products.Interface;
using shopping_app.Utilities;

namespace shopping_app;

internal abstract class Program
{
    private static readonly WoolworthsParser WooliesParser = new();
    private static readonly PnpParser PnpParser = new();
    private static readonly CheckersParser CheckersParser = new();

    public static async Task Main(string[] args)
    {
        #region Woolies

        // const string wooliesUrl = "https://www.woolworths.co.za/cat/Food/_/N-1z13sk5";
        //
        // var wooliesProducts = await GetProductsAsync(wooliesUrl, true);
        // Console.WriteLine(
        //     $"{wooliesProducts.Count} Woolies products found, {wooliesProducts.Count(x => !string.IsNullOrWhiteSpace(x.Special))} Specials Available");

        #endregion

        #region PnP

        // var pnpUrl = "https://www.pnp.co.za/search/all";

        // var pnpProducts = await GetProductsAsync(pnpUrl);
        // Console.WriteLine($"{pnpProducts.Count} PnP products found, {pnpProducts.Count(x => !string.IsNullOrWhiteSpace(x.Special))} Specials Available");

        #endregion

        #region Checkers

        // const string checkersUrl = "https://www.checkers.co.za/c-2413/All-Departments/Food?q=%3Arelevance%3AbrowseAllStoresFacetOff%3AbrowseAllStoresFacetOff";
        //
        // var checkersProducts = await GetProductsAsync(checkersUrl, true);
        // Console.WriteLine($"{checkersProducts.Count} Checkers products found, " +
        //                   $"{checkersProducts.Count(x => !string.IsNullOrWhiteSpace(x.Special))} Specials Available");

        #endregion

        #region Evaluate

        var path = @"C:\Users\justi\Documents";

        var wooliesRaw = await File.ReadAllTextAsync($@"{path}\Woolworths.json");
        var wooliesProducts = JsonConvert.DeserializeObject<List<WoolworthsProduct>>(wooliesRaw);
        if (wooliesProducts == null) return;
        var wooliesItemNames = wooliesProducts.Select(x => x.Name).ToList();

        var checkersRaw = await File.ReadAllTextAsync($@"{path}\Checkers.json");
        var checkersProducts = JsonConvert.DeserializeObject<List<CheckersProduct>>(checkersRaw);
        if (checkersProducts == null) return;
        var checkersItemNames = checkersProducts.Select(x => x.Name).ToList();

        var similaritiesJson = await File.ReadAllTextAsync($@"{path}\Similarities.json");
        var similarities = JsonConvert.DeserializeObject<List<(double, string?, string?)>>(similaritiesJson);
        if (similarities == null) return;

        var priceComparisons = new List<PriceComparison>();
        foreach (var similarity in similarities)
        {
            priceComparisons.Add(new PriceComparison
            {
                WoolworthsName = similarity.Item2,
                WoolworthsPrice = wooliesProducts.FirstOrDefault(x => x.Name == similarity.Item2)?.Price,
                CheckersName = similarity.Item3,
                CheckersPrice = checkersProducts.FirstOrDefault(x => x.Name == similarity.Item3)?.Price
            });
        }

        Console.WriteLine($"{priceComparisons.Count} price comparisons created");
        if (priceComparisons.Count == 0) return;
        var json = JsonConvert.SerializeObject(priceComparisons, Formatting.Indented);
        await File.WriteAllTextAsync($@"{path}\FullPriceComparisons.json", json);

        #region CreateSimilaritiesFile

        // var evaluatedSimilarities = Similarities.Evaluate(wooliesItemNames, checkersItemNames);
        // Console.WriteLine($"{evaluatedSimilarities.Count} similarities found");
        // if (evaluatedSimilarities.Count > 0)
        // {
        //     var json = JsonConvert.SerializeObject(evaluatedSimilarities, Formatting.Indented);
        //     await File.WriteAllTextAsync(@"C:\Users\justi\Documents\Similarities.json", json);
        // }

        #endregion

        #endregion
    }

    private static async Task<List<IProduct>> GetProductsAsync(string url, bool searchMultiplePages = false)
    {
        var htmlDocs = await RequestManager.GetHtmlFromUrl(url, searchMultiplePages);
        if (htmlDocs == null || htmlDocs.Count == 0) return [];

        List<IProduct> products = [];
        switch (url)
        {
            case var _ when url.Contains("woolworths.co.za"):
                foreach (var htmlDoc in htmlDocs)
                    products.AddRange(WooliesParser.GetProducts(htmlDoc, true));

                await SaveToFileAsync("Woolworths", products);
                break;
            case var _ when url.Contains("pnp.co.za"):
                foreach (var htmlDoc in htmlDocs)
                    products.AddRange(PnpParser.GetProducts(htmlDoc, true));
                break;
            case var _ when url.Contains("checkers.co.za"):
                foreach (var htmlDoc in htmlDocs)
                    products.AddRange(CheckersParser.GetProducts(htmlDoc, true));

                await SaveToFileAsync("Checkers", products);

                break;
        }

        return products;
    }

    private static async Task SaveToFileAsync(string fileName, List<IProduct> products)
    {
        try
        {
            var path = $@"C:\Users\justi\Documents\{fileName}.json";
            Console.WriteLine($"Saving {products.Count} products to {path}");
            await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(products));
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to save products!\n" +
                              $"{e.Message}");
            throw;
        }
    }
}