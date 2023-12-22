namespace AvitoParser;

public static class ClientConfigurator
{
    private const string baseAddress = "https://www.avito.ru";

    private static readonly Dictionary<string, string> defaultHeaders;
    private static readonly string[] userAgents;
    private static readonly Random random;

    static ClientConfigurator()
    {
        random = new Random();

        defaultHeaders = new Dictionary<string, string>
        {
            { "Accept", "application/json" },
            { "Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3" },
            { "Cache-Control", "no-cache" },
            { "Connection", "keep-alive" },
            { "Host", "www.avito.ru" },
            { "Pragma", "no-cache" },
            { "User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:109.0) Gecko/20100101 Firefox/119.0" }
        };

        userAgents = ResourceLoader.LoadUserAgents();
    }

    public static void ConfigureClient(HttpClient client)
    {
        foreach (var (header, value) in defaultHeaders)
            client.DefaultRequestHeaders.Add(header, value);

        client.BaseAddress = new Uri(baseAddress);
    }

    public static void SetRandomUserAgent(HttpClient client)
    {
        var randomPosition = random.Next(0, userAgents.Length);
        client.DefaultRequestHeaders.Remove("User-Agent");
        client.DefaultRequestHeaders.Add("User-Agent", userAgents[randomPosition]);
    }
}