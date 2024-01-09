using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Text.RegularExpressions;
using static AvitoParser.Configuration;

namespace AvitoParser;

public static class ParserHelper
{
    private static readonly Regex pageRegex = new(@"p=(\d+)", RegexOptions.Compiled);

    public static int GetLastPageNumber(HtmlNode root)
    {
        var rawNumber = root
            .CssSelect(LastPageButtonClass)
            .First()
            .InnerText;

        return int.Parse(rawNumber);
    }

    public static int GetCurrentPageNumber(string url)
    {
        var match = pageRegex.Match(url);
        return match.Success ? int.Parse(match.Groups[1].Value) : 1;
    }

    public static string GetNextPageUrl(string currentUrl)
    {
        var match = pageRegex.Match(currentUrl);

        currentUrl = match.Success switch
        {
            true => pageRegex.Replace(currentUrl, $"p={int.Parse(match.Groups[1].Value) + 1}"),
            _ => currentUrl + "&p=" + 2
        };

        return currentUrl;
    }

    public static IEnumerable<HtmlNode> GetCardsNodes(HtmlNode root)
    {
        return root.CssSelect(CardClass);
    }
}