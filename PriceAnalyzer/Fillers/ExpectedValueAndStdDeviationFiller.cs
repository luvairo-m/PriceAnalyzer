using AvitoParser;
using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public class ExpectedValueAndStdDeviationFiller : IResponseFiller
{
    public void FillResponse(ParseResponse response)
    {
        var adverts = response.Advertisements;

        var expectedValue = adverts.Average(advert => advert.Price);
        var stdDeviation = GetStandardDeviation(adverts, expectedValue);

        response.ExpectedValue = expectedValue;
        response.StandardDeviation = stdDeviation;
    }

    private static double GetStandardDeviation(List<Advertisement> adverts, double expectedValue)
    {
        var squareSum = 0.0;

        foreach (var advert in adverts)
        {
            var deviation = advert.Price - expectedValue;
            squareSum += deviation * deviation;
        }

        return Math.Sqrt(squareSum / (adverts.Count - 1));
    }
}