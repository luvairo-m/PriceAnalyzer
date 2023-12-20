using System.Reflection;

namespace AvitoParser;

public static class ConfigurationHelper
{
    public static readonly Dictionary<string, string> DefaultHeaders;
    private static readonly string[] userAgents;
    private static readonly Random random;

    static ConfigurationHelper()
    {
        random = new Random();
        
        DefaultHeaders = new Dictionary<string, string>
        {
            { "Accept", "application/json" },
            { "Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3" },
            { "Cache-Control", "no-cache" },
            { "Connection", "keep-alive" },
            { "Host", "www.avito.ru" },
            { "Pragma", "no-cache" },
            { "User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:109.0) Gecko/20100101 Firefox/119.0" }
        };

        userAgents = LoadUserAgents();
    }

    private static string[] LoadUserAgents()
    {
        const char separator = '\n';
        
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"{nameof(AvitoParser)}.Resources.agents.txt");

        using var reader = new StreamReader(stream!);
        var agents = reader.ReadToEnd().Split(separator);

        return agents;
    }

    private static string GetRandomUserAgent()
    {
        var randomPosition = random.Next(0, userAgents.Length);
        return userAgents[randomPosition];
    }
}