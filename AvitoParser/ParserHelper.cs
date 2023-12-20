namespace AvitoParser;

public static class ParserHelper
{
    private static readonly string[] months =
    {
        "января", "февраля", "марта",
        "апреля", "мая", "июня",
        "июля", "августа", "сентября",
        "октября", "ноября", "декабря"
    };

    public static (string region, string city) GetLocationSummary(string location)
    {
        var locationParts = location.Split(", ");

        var possibleRegion = locationParts[0];
        var possibleCity = locationParts[1];

        if (!possibleRegion.Contains(' '))
            possibleCity = possibleRegion;

        return (possibleRegion, possibleCity);
    }

    public static DateTime CombineDateTimeFrom(string dateString)
    {
        var dateParts = dateString.Split(' ');
        return dateParts[^1] == "назад" ? HardParsingStrategy(dateParts) : SimpleParsingStrategy(dateParts);

        static DateTime SimpleParsingStrategy(IReadOnlyList<string> parts)
        {
            var day = int.Parse(parts[0]);
            var month = Array.IndexOf(months, parts[1]) + 1;
            var year = int.Parse(parts[2]);

            return new DateTime(year, month, day);
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
}