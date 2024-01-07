using AvitoParser;
using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public class MedianDeviationFiller : IResponseFiller
{
    public void FillResponse(ParseResponse response)
    {
        var adverts = response.Advertisements;
        var medianPrice = GetMedianPrice(adverts);

        response.MedianPrice = medianPrice;
        adverts.ForEach(advert =>
            advert.DeviationFromMedian = FillerHelper.GetDeviationPercent(advert.Price, medianPrice));
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