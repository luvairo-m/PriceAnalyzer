using HtmlAgilityPack;

namespace AvitoParser;

public class Parser
{
    private readonly HttpClient httpClient;

    public Parser(IHttpClientFactory clientFactory)
    {
        httpClient = clientFactory.CreateClient();
        ClientConfigurator.ConfigureClient(httpClient);
    }

    public async Task<IList<Advertisement>> GetAdvertisements(string url, int cardAmount)
    {
        var adverts = new List<Advertisement>(cardAmount);

        var document = await GetHtmlDocument(url);
        var root = document.DocumentNode;

        var pagesRemaining = Helper.GetLastPageNumber(root);

        while (cardAmount > 0)
        {
            if (pagesRemaining == 0)
                return adverts;

            foreach (var cardNode in Helper.GetCardsNodes(root))
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

            url = Helper.GetNextPageUrl(url);
            pagesRemaining -= 1;

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