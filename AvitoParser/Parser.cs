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

    public async Task<List<Advertisement>> GetAdvertisements(string url, int cardAmount)
    {
        var adverts = new List<Advertisement>(cardAmount);

        var document = await GetHtmlDocument(url);
        var root = document.DocumentNode;

        var currentPageNumber = ParserHelper.GetCurrentPageNumber(url);
        var lastPageNumber = ParserHelper.GetLastPageNumber(root);

        while (cardAmount > 0)
        {
            if (currentPageNumber > lastPageNumber)
                break;

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
                cardAmount -= 1;
            }

            ClientConfigurator.SetRandomUserAgent(httpClient);

            url = ParserHelper.GetNextPageUrl(url);
            currentPageNumber += 1;

            // This line will be changed in future.
            await Task.Delay(random.Next(7500, 10_000));

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