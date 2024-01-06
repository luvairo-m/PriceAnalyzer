using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public class AverageDeviationFiller : IParseResponseFiller
{
    public void FillResponse(ParseResponse response)
    {
        var adverts = response.Advertisements;

        var averagePrice = adverts.Average(advert => advert.Price);
        response.AveragePrice = Math.Round(averagePrice, 3);

        foreach (var advert in adverts)
        {
            var deviation = (advert.Price - averagePrice) / averagePrice * 100;
            advert.PriceDeviationFromAverage = (int)Math.Round(deviation);
        }
    }
}