using HtmlAgilityPack;
using ScrapySharp.Extensions;
using static AvitoParser.Configuration;

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
        
        while (cardAmount > 0)
        {
            var document = await GetHtmlDocument(url);
            var root = document.DocumentNode;
            
            foreach (var cardNode in GetCardsNodes(root))
            {
                var advert = new AdvertisementBuilder(cardNode)
                    .SetTitle()
                    .SetPrice()
                    .SetLocation()
                    .SetUrl()
                    .SetPublicationDate()
                    .Build();

                adverts.Add(advert);

                if (cardAmount-- < 0)
                    return adverts;

                ClientConfigurator.SetRandomUserAgent(httpClient);
            }
        }

        return adverts;
    }

    private static IEnumerable<HtmlNode> GetCardsNodes(HtmlNode root)
    {
        return root.CssSelect(CardClass);
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