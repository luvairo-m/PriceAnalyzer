using System.Reflection;

namespace AvitoParser;

public static class ResourceLoader
{
    public static string[] LoadResourcesByName(string resourceName)
    {
        var separators = new[] { '\n', '\r' };
        
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"{nameof(AvitoParser)}.Resources.{resourceName}");

        using var reader = new StreamReader(stream!);
        var resources = reader.ReadToEnd().Split(separators, StringSplitOptions.RemoveEmptyEntries);

        return resources;
    }
}