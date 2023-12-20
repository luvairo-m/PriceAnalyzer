namespace AvitoParser;

public static class ParserHelper
{
    public static (string region, string city) GetLocationSummary(string location)
    {
        if (location == null)
            throw new NullReferenceException("Location can't be null");

        var locationParts = location.Split(',');

        if (locationParts.Length < 2)
            throw new Exception("Incorrect location");

        var possibleRegion = locationParts[0];
        var possibleCity = locationParts[1];

        if (!possibleRegion.Contains(' '))
            possibleRegion = possibleCity;

        return (possibleRegion, possibleCity);
    }
}