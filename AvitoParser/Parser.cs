using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace AvitoParser;

public class Parser : IParser<Advertisement>
{
    private const string baseAddress = "https://www.avito.ru";

    private const string cardClass = ".iva-item-root-_lk9K";
    private const string cardDataContainerClass = ".iva-item-body-KLUuy";
    private const string cardTitleClass = ".iva-item-titleStep-pdebR";
    private const string cardPriceClass = ".iva-item-priceStep-uq2CQ";
    private const string cardDateClass = ".iva-item-dateInfoStep-_acjp";
    private const string cardLocationClass = ".style-item-address__string-wt61A";

    private readonly HttpClient httpClient;

    public Parser(IHttpClientFactory clientFactory)
    {
        httpClient = clientFactory.CreateClient();
        ConfigureClient(httpClient);
    }

    public async Task<IList<Advertisement>> GetParsedData(string url, int amount)
    {
        var document = await GetHtmlDocument(url);
        var root = document.DocumentNode;

        var adverts = new List<Advertisement>(amount);

        while (amount > 0)
        {
            var cardNodes = GetCardsNodes(root);

            foreach (var cardNode in cardNodes)
            {
                var cardPrice = GetCardPrice(root);
                var cardLocation = GetCardLocation(root);

                var (cardTitle, cardUrl) = GetCardSummary(root);
                var (cardRegion, cardCity) = ParserHelper.GetLocationSummary(cardLocation);

                // process card

                amount--;

                if (amount > 0)
                    continue;

                return adverts;
            }

            // cardNodes = FindNextPage(...)
        }

        return adverts;
    }

    private static (string? title, string? url) GetCardSummary(HtmlNode cardNode)
    {
        var container = cardNode
            .CssSelect($"{cardDataContainerClass} {cardTitleClass} a")
            .FirstOrDefault();

        var title = container.GetAttributeValue("title");
        var url = baseAddress + container.GetAttributeValue("href");

        return (title, url);
    }

    private static int? GetCardPrice(HtmlNode cardNode)
    {
        var rawPrice = cardNode
            .CssSelect($"{cardDataContainerClass} {cardPriceClass} p meta")
            .Skip(1)
            .FirstOrDefault()?
            .GetAttributeValue("content");

        if (int.TryParse(rawPrice, out var price))
            return price;

        return null;
    }

    private static string GetCardLocation(HtmlNode cardNode)
    {
        var location = cardNode
            .CssSelect($"span[class='{cardLocationClass}']")
            .First()
            .InnerText;

        return location;
    }

    private static string GetCardPublicationDate(HtmlNode cardNode)
    {
        var dateString = cardNode
            .CssSelect($"{cardDataContainerClass} {cardDateClass} p")
            .First()
            .InnerText;

        return dateString;
    }

    private static IEnumerable<HtmlNode> GetCardsNodes(HtmlNode root)
    {
        return root.CssSelect(cardClass);
    }

    private async Task<HtmlDocument> GetHtmlDocument(string url)
    {
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var document = new HtmlDocument();
        document.LoadHtml(await response.Content.ReadAsStringAsync());

        return document;
    }

    private static void ConfigureClient(HttpClient client)
    {
        foreach (var (header, value) in ConfigurationHelper.DefaultHeaders)
            client.DefaultRequestHeaders.Add(header, value);

        client.BaseAddress = new Uri(baseAddress);
    }
}