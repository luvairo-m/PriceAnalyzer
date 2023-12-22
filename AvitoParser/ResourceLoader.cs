using System.Reflection;

namespace AvitoParser;

public static class ResourceLoader
{
    public static string[] LoadCities()
    {
        const char separator = '\n';

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"{nameof(AvitoParser)}.Resources.cities.txt");

        using var reader = new StreamReader(stream!);
        var cities = reader.ReadToEnd().Split(separator);

        return cities;
    }

    public static string[] LoadUserAgents()
    {
        const char separator = '\n';

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"{nameof(AvitoParser)}.Resources.agents.txt");

        using var reader = new StreamReader(stream!);
        var agents = reader.ReadToEnd().Split(separator);

        return agents;
    }
}