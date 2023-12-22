namespace AvitoParser.Helpers;

public static class DateTimeHelper
{
    private static readonly string[] months;

    static DateTimeHelper()
    {
        months = new[]
        {
            "января", "февраля", "марта",
            "апреля", "мая", "июня",
            "июля", "августа", "сентября",
            "октября", "ноября", "декабря"
        };
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
}