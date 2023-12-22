using HtmlAgilityPack;
using ScrapySharp.Extensions;
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

        var title = container.GetAttributeValue("title")!;
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

        advertisement.PublicationDate = BuilderHelper.CombineDateTimeFrom(dateString);
        return this;
    }

    public AdvertisementBuilder SetLocation()
    {
        return this;
    }

    public Advertisement Build()
    {
        return advertisement;
    }
}