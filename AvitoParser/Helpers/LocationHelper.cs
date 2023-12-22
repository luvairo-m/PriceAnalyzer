namespace AvitoParser.Helpers;

public static class LocationHelper
{
    private static readonly Dictionary<string, string> replacements;
    private static readonly string[] cities;

    static LocationHelper()
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

        cities = ResourceLoader.LoadResourcesByName("cities.txt");
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