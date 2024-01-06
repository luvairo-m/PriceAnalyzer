using AvitoParser;
using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public class MedianDeviationFiller : IParseResponseFiller
{
    public void FillResponse(ParseResponse response)
    {
        var adverts = response.Advertisements;

        var medianPrice = GetMedianPrice(adverts);
        response.MedianPrice = Math.Round(medianPrice, 3);

        foreach (var advert in adverts)
        {
            var deviation = (advert.Price - medianPrice) / medianPrice * 100;
            advert.PriceDeviationFromMedian = (int)Math.Round(deviation);
        }
    }

    private static double GetMedianPrice(IEnumerable<Advertisement> adverts)
    {
        var tempList = adverts
            .Select(advert => advert.Price)
            .ToList();

        tempList.Sort();

        var size = tempList.Count;
        var middleIndex = size / 2;

        if (size % 2 == 0)
            return (tempList[middleIndex - 1] + (double)tempList[middleIndex]) / 2;

        return tempList[middleIndex];
    }
}