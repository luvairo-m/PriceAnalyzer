using AvitoParser.Helpers;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Text;
using System.Web;
using static AvitoParser.Configuration;

namespace AvitoParser;

public class AdvertisementBuilder
{
    private readonly Advertisement advertisement;
    private readonly HtmlNode root;

    public AdvertisementBuilder(HtmlNode root)
    {
        advertisement = new Advertisement();
        this.root = root;
    }

    public AdvertisementBuilder SetUrl()
    {
        var container = root
            .CssSelect($"{CardDataContainerClass} {CardTitleClass} a")
            .First();

        var url = BaseAddress + container.GetAttributeValue("href");
        advertisement.Url = url;

        return this;
    }

    public AdvertisementBuilder SetTitle()
    {
        var container = root
            .CssSelect($"{CardDataContainerClass} {CardTitleClass} a")
            .First();

        var title = HttpUtility.HtmlDecode(container.GetAttributeValue("title")!);
        advertisement.Title = title;

        return this;
    }

    public AdvertisementBuilder SetPrice()
    {
        var rawPrice = root
            .CssSelect($"{CardDataContainerClass} {CardPriceClass} p meta")
            .Skip(1)
            .First()
            .GetAttributeValue("content");

        advertisement.Price = int.Parse(rawPrice);
        return this;
    }

    public AdvertisementBuilder SetPublicationDate()
    {
        var dateString = root
            .CssSelect($"{CardDataContainerClass} {CardDateClass} p")
            .First()
            .InnerText;

        advertisement.PublicationDate = DateTimeHelper.CombineDateTimeFrom(dateString);
        return this;
    }

    public AdvertisementBuilder SetActualLocation()
    {
        var fragments = new List<string>();
        var locationBuilder = new StringBuilder();

        foreach (var field in root.CssSelect($"{ActualLocationClass} p"))
        foreach (var innerField in field.CssSelect("span"))
        {
            var textData = innerField.InnerText.TrimStart().TrimEnd();
            fragments.Add(textData);

            if (textData.Contains("·"))
                fragments.Clear();
        }

        advertisement.Location = string.Join("; ", fragments);
        return this;
    }

    public Advertisement Build()
    {
        return advertisement;
    }
}