using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Text.RegularExpressions;
using static AvitoParser.Configuration;

namespace AvitoParser;

public static class Helper
{
    private static readonly Dictionary<string, string> replacements;
    private static readonly string[] months;
    private static readonly string[] cities;

    static Helper()
    {
        replacements = new Dictionary<string, string>
        {
            { "eh", "э" }, { "yu", "ю" }, { "jj", "й" },
            { "ch", "ч" }, { "sh", "ш" }, { "h", "х" },
            { "shh", "щ" }, { "zh", "ж" }, { "jo", "ё" },
            { "ya", "я" }, { "a", "а" }, { "b", "б" },
            { "v", "в" }, { "g", "г" }, { "d", "д" },
            { "e", "е" }, { "z", "з" }, { "i", "и" },
            { "k", "к" }, { "l", "л" }, { "m", "м" },
            { "n", "н" }, { "o", "о" }, { "p", "п" },
            { "r", "р" }, { "s", "с" }, { "t", "т" },
            { "u", "у" }, { "f", "ф" }, { "c", "ц" },
            { "'", "ъ" }, { "y", "ы" }, { "_", " " }
        };

        months = new[]
        {
            "января", "февраля", "марта",
            "апреля", "мая", "июня",
            "июля", "августа", "сентября",
            "октября", "ноября", "декабря"
        };

        cities = ResourceLoader.LoadCities();
    }

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

    public static DateTime CombineDateTimeFrom(string dateString)
    {
        var dateParts = dateString.Split(' ');
        return dateParts[^1] == "назад" ? HardParsingStrategy(dateParts) : SimpleParsingStrategy(dateParts);

        static DateTime SimpleParsingStrategy(IReadOnlyList<string> parts)
        {
            var day = int.Parse(parts[0]);
            var month = Array.IndexOf(months, parts[1]) + 1;
            return new DateTime(DateTime.Now.Year, month, day);
        }

        static DateTime HardParsingStrategy(IReadOnlyList<string> parts)
        {
            var timeDelta = -int.Parse(parts[0]);

            var dateTime = parts[1][0] switch
            {
                'м' => DateTime.Now.AddMinutes(timeDelta),
                'ч' => DateTime.Now.AddHours(timeDelta),
                'д' => DateTime.Now.AddDays(timeDelta),
                'н' => DateTime.Now.AddDays(timeDelta * 7),
                _ => throw new Exception("Unknown format")
            };

            return dateTime;
        }
    }

    public static string GetCityFromUrl(string url)
    {
        var urlParts = url.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return urlParts[2] == "all" ? "Все регионы" : TransliterateBack(urlParts[2]);
    }

    private static string TransliterateBack(string rawCity)
    {
        rawCity = replacements.Keys.Aggregate(rawCity,
            (current, key) => current.Replace(key, replacements[key]));

        return GetAppropriateCity(rawCity);
    }

    private static string GetAppropriateCity(string input)
    {
        var (bestCity, bestDistance) = (string.Empty, int.MaxValue);

        foreach (var city in cities)
        {
            var distance = CalculateLevenshteinDistance(input, city);

            if (distance == 0)
                return city;

            if (distance >= bestDistance)
                continue;

            bestCity = city;
            bestDistance = distance;
        }

        return bestCity;
    }

    private static int CalculateLevenshteinDistance(string s, string t)
    {
        var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

        var matrix = new int[bounds.Height, bounds.Width];

        for (var height = 0; height < bounds.Height; height++)
            matrix[height, 0] = height;

        for (var width = 0; width < bounds.Width; width++)
            matrix[0, width] = width;

        for (var height = 1; height < bounds.Height; height++)
        for (var width = 1; width < bounds.Width; width++)
        {
            var cost = s[height - 1] == t[width - 1] ? 0 : 1;

            matrix[height, width] = Math.Min(
                Math.Min(matrix[height - 1, width] + 1, matrix[height, width - 1] + 1),
                matrix[height - 1, width - 1] + cost);
        }

        return matrix[bounds.Height - 1, bounds.Width - 1];
    }
}