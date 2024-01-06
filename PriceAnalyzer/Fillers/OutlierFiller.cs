using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public class OutlierFiller : IParseResponseFiller
{
    public void FillResponse(ParseResponse response)
    {
        var adverts = response.Advertisements;
        var prices = adverts
            .Select(advert => advert.Price)
            .ToList();

        prices.Sort();

        var (lower, upper) = GetBounds(prices);

        foreach (var advert in adverts)
            if (advert.Price < lower || advert.Price > upper)
                advert.IsOutlier = true;
    }

    private static (double lowerBound, double upperBound) GetBounds(List<int> prices)
    {
        const double iqrScale = 1.5;

        var q1 = prices[(int)(0.25 * prices.Count)];
        var q3 = prices[(int)(0.75 * prices.Count)];
        var iqr = q3 - q1;

        return (q1 - iqr * iqrScale, q3 + iqr * iqrScale);
    }
}