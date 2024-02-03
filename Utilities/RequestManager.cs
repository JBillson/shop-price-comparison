using HtmlAgilityPack;

namespace shopping_app.Utilities;

public static class RequestManager
{
    public static async Task<List<HtmlDocument>?> GetHtmlFromUrl(string url, bool searchMultiplePages = false)
    {
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
                    string page;
                    if (url.Contains("checkers", StringComparison.InvariantCulture))
                        page = $"&page={pageNumber}";
                    else if (url.Contains("woolworths", StringComparison.InvariantCulture))
                    {
                        var nrpp = 25;
                        page = $"?No={(pageNumber - 1) * nrpp}&Nrpp={nrpp}";
                    }
                    else
                    {
                        throw new Exception($"Haven't handled multi-page search for this url: {url}");
                    }

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