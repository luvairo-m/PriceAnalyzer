using AvitoParser;
using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public class ModeFiller : IResponseFiller
{
    public void FillResponse(ParseResponse response)
    {
        var adverts = response.Advertisements;
        var counts = GetPriceCounts(adverts);

        var maxFrequency = counts.Values.Max();
        var results = counts
            .Where(pair => pair.Value == maxFrequency)
            .Select(pair => pair.Key)
            .ToList();

        response.PriceMode = results.Average();
    }

    private static Dictionary<int, int> GetPriceCounts(IEnumerable<Advertisement> adverts)
    {
        return adverts
            .GroupBy(advert => advert.Price)
            .ToDictionary(group => group.Key, group => group.Count());
    }
}