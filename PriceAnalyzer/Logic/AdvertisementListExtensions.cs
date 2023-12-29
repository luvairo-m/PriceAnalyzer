using AvitoParser;

namespace PriceAnalyzer.Logic;

public static class AdvertisementListExtensions
{
    public static double GetMedianPrice(this List<Advertisement> adverts)
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

    public static void FillPriceDeviation(this List<Advertisement> adverts, double medianPrice)
    {
        foreach (var advert in adverts)
        {
            var deviation = (advert.Price - medianPrice) / medianPrice * 100;
            advert.PriceDeviation = (int)Math.Round(deviation);
        }
    }
}