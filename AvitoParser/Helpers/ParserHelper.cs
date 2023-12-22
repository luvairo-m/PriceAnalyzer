using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Text.RegularExpressions;
using static AvitoParser.Configuration;

namespace AvitoParser;

public static class ParserHelper
{
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
        var regex = new Regex(@"p=(\d+)", RegexOptions.Compiled);
        var match = regex.Match(url);
        return match.Success ? int.Parse(match.Groups[1].Value) : 1;
    }

    public static string GetNextPageUrl(string currentUrl)
    {
        var regex = new Regex(@"p=(\d+)", RegexOptions.Compiled);
        var match = regex.Match(currentUrl);

        if (match.Success)
            currentUrl = regex.Replace(currentUrl, $"p={int.Parse(match.Groups[1].Value) + 1}");
        else
            currentUrl = currentUrl + "&p=" + 2;

        return currentUrl;
    }

    public static IEnumerable<HtmlNode> GetCardsNodes(HtmlNode root)
    {
        return root.CssSelect(CardClass);
    }
}