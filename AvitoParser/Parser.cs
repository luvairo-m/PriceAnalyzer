using HtmlAgilityPack;

namespace AvitoParser;

public class Parser
{
    private static readonly Random random;
    private readonly HttpClient httpClient;

    static Parser()
    {
        random = new Random();
    }

    public Parser(HttpClient client)
    {
        httpClient = client;
    }

    public async Task<IList<Advertisement>> GetAdvertisements(string url, int cardAmount)
    {
        var adverts = new List<Advertisement>(cardAmount);

        var document = await GetHtmlDocument(url);
        var root = document.DocumentNode;

        var currentPageNumber = ParserHelper.GetCurrentPageNumber(url);
        var pagesAmount = ParserHelper.GetLastPageNumber(root);

        while (cardAmount > 0)
        {
            if (currentPageNumber > pagesAmount)
                return adverts;

            foreach (var cardNode in ParserHelper.GetCardsNodes(root))
            {
                if (cardAmount == 0)
                    return adverts;

                var advert = new AdvertisementBuilder(cardNode)
                    .SetTitle()
                    .SetPrice()
                    .SetUrl()
                    .SetLocation()
                    .SetPublicationDate()
                    .Build();

                adverts.Add(advert);
                ClientConfigurator.SetRandomUserAgent(httpClient);

                cardAmount -= 1;
            }

            url = ParserHelper.GetNextPageUrl(url);
            currentPageNumber += 1;

            // This line will be changed in future.
            await Task.Delay(random.Next(2500, 7500));

            document = await GetHtmlDocument(url);
            root = document.DocumentNode;
        }

        return adverts;
    }

    private async Task<HtmlDocument> GetHtmlDocument(string url)
    {
        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var document = new HtmlDocument();
        document.LoadHtml(await response.Content.ReadAsStringAsync());

        return document;
    }
}