using AvitoParser;

namespace PriceAnalyzer;

public class ParsingClient
{
    public ParsingClient(HttpClient client)
    {
        Client = client;
        ClientConfigurator.ConfigureClient(client);
    }

    public HttpClient Client { get; }
}