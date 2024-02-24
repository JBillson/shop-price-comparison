using HtmlAgilityPack;
using shopping_app.Products.Enums;

namespace shopping_app.Utilities;

public static class RequestManager
{
    private static readonly HtmlWeb Web = new();
    
    public static async Task<List<HtmlDocument>?> ScrapeShop(Shop shop)
    {
        var url = shop switch
        {
            Shop.Checkers => "https://www.checkers.co.za/c-2413/All-Departments/Food?q=%3Arelevance%3AbrowseAllStoresFacetOn%3AbrowseAllStoresFacetOn",
            Shop.Woolworths => "https://www.woolworths.co.za/cat/Food/_/N-1z13sk5",
            Shop.PickNPay => "https://www.pnp.co.za/search/all",
            _ => throw new ArgumentOutOfRangeException(nameof(shop), shop, null)
        };

        return await ScrapeMultiplePages(shop, url);
    }

    private static async Task<List<HtmlDocument>> ScrapeMultiplePages(Shop shop, string url)
    {
        const int pageLimit = 100;
        List<HtmlDocument> htmlDocs = [];
        var pageNumber = 1;
        
        while (pageNumber < pageLimit)
        {
            try
            {
                var page = shop switch
                {
                    Shop.Woolworths => $"?No={(pageNumber - 1) * 25}&Nrpp=25",
                    Shop.Checkers => $"&page={pageNumber}",
                    _ => throw new ArgumentOutOfRangeException(nameof(shop), shop, null)
                };

                Console.WriteLine($"Scraping {url}{page}");

                var htmlDoc = await Web.LoadFromWebAsync(url + page);
                if (htmlDoc == null)
                {
                    Console.WriteLine(
                        $"Stopping multiple page search at page {pageNumber} due to no Html Doc being returned.");
                    break;
                }

                htmlDocs.Add(htmlDoc);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Stopping multiple page search at page {pageNumber} due to exception:\n" +
                                  $"{e.Message}");
                break;
            }

            pageNumber++;
        }

        return htmlDocs;
    }
}