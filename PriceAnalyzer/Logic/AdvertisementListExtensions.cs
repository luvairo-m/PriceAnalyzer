using AvitoParser;

namespace PriceAnalyzer.Logic;

public static class AdvertisementListExtensions
{
    public static double FillPriceDeviation(this IList<Advertisement> adverts)
    {
        var averagePrice = adverts.Average(advert => advert.Price);

        foreach (var advert in adverts)
        {
            var deviation = (advert.Price - averagePrice) / averagePrice * 100;
            advert.PriceDeviation = (int)Math.Round(deviation);
        }

        return Math.Round(averagePrice, 3);
    }
}