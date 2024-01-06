using AvitoParser;
using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public class ExpectedValueAndStdDeviationFiller : IParseResponseFiller
{
    public void FillResponse(ParseResponse response)
    {
        var adverts = response.Advertisements;

        var expectedValue = GetExpectedValue(adverts);
        var standardDeviation = GetStandardDeviation(adverts, expectedValue);

        response.ExpectedValue = Math.Round(expectedValue, 3);
        response.StandardDeviation = Math.Round(standardDeviation, 3);
    }

    private static double GetStandardDeviation(List<Advertisement> adverts, double expectedValue)
    {
        var squareSum = 0.0;

        foreach (var advert in adverts)
        {
            var deviation = advert.Price - expectedValue;
            squareSum += deviation * deviation;
        }

        return Math.Sqrt(squareSum / adverts.Count);
    }

    private static double GetExpectedValue(List<Advertisement> adverts)
    {
        var counts = GetPriceCounts(adverts);
        var expectedValue = 0.0;

        foreach (var advert in adverts)
            expectedValue += advert.Price * ((double)counts[advert.Price] / adverts.Count);

        return expectedValue;
    }

    private static Dictionary<int, int> GetPriceCounts(List<Advertisement> adverts)
    {
        var counts = new Dictionary<int, int>();

        foreach (var advert in adverts)
            if (counts.ContainsKey(advert.Price))
                counts[advert.Price]++;
            else
                counts[advert.Price] = 1;

        return counts;
    }
}