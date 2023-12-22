using System.Reflection;

namespace AvitoParser;

public static class ResourceLoader
{
    public static string[] LoadResourcesByName(string resourceName)
    {
        const char separator = '\n';

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"{nameof(AvitoParser)}.Resources.{resourceName}");

        using var reader = new StreamReader(stream!);
        var resources = reader.ReadToEnd().Split(separator);

        return resources;
    }
}