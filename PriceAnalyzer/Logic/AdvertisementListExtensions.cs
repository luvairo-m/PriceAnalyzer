using AvitoParser;

namespace PriceAnalyzer.Logic;

public static class AdvertisementListExtensions
{
    public static void FillPriceDeviation(this IList<Advertisement> adverts)
    {
        var averagePrice = adverts.Average(advert => advert.Price);

        foreach (var advert in adverts)
        {
            var deviation = Math.Abs((advert.Price - averagePrice) / averagePrice) * 100;
            advert.PriceDeviation = deviation;
        }
    }
}