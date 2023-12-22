using HtmlAgilityPack;

namespace AvitoParser;

public class Parser
{
    private readonly HttpClient httpClient;
    private readonly Random random;

    public Parser(HttpClient client)
    {
        httpClient = client;
        random = new Random();

        ClientConfigurator.ConfigureClient(httpClient);
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

            await Task.Delay(random.Next(2500, 7000));

            document = await GetHtmlDocument(url);
            root = document.DocumentNode;
        }

        return adverts;
    }

    private async Task<HtmlDocument> GetHtmlDocument(string url)
    {
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var document = new HtmlDocument();
        document.LoadHtml(await response.Content.ReadAsStringAsync());

        return document;
    }
}