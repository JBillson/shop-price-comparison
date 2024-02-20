using HtmlAgilityPack;
using shopping_app.Products.Enums;

namespace shopping_app.Utilities;

public static class RequestManager
{
    public static async Task<List<HtmlDocument>?> ScrapeShop(Shop shop, bool searchMultiplePages = false)
    {
        var url = shop switch
        {
            Shop.Checkers => "https://www.checkers.co.za/c-2413/All-Departments/Food?q=%3Arelevance%3AbrowseAllStoresFacetOn%3AbrowseAllStoresFacetOn",
            Shop.Woolworths => "https://www.woolworths.co.za/cat/Food/_/N-1z13sk5",
            Shop.PickNPay => "https://www.pnp.co.za/search/all",
            _ => throw new ArgumentOutOfRangeException(nameof(shop), shop, null)
        };
        
        const int pageLimit = 100;
        List<HtmlDocument> htmlDocs = [];
        var web = new HtmlWeb();
        if (searchMultiplePages)
        {
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

                    var htmlDoc = await web.LoadFromWebAsync(url + page);
                    if (htmlDoc == null)
                    {
                        Console.WriteLine(
                            $"Stopping multiple page search at page {pageNumber} due to no Html Doc being returned.");
                        throw new Exception("Probably been flagged as a bot");
                    }

                    htmlDocs.Add(htmlDoc);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Stopping multiple page search at page {pageNumber} due to exception:\n" +
                                      $"{e.Message}");
                    throw;
                }

                pageNumber++;
            }
        }
        else
        {
            try
            {
                htmlDocs.Add(await web.LoadFromWebAsync(url));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to load url: {url}" +
                                  $"{e.Message}");
                throw;
            }
        }

        return htmlDocs;
    }

}