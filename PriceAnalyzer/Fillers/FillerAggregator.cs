using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public static class FillerAggregator
{
    private static readonly List<IParseResponseFiller> fillers = new();

    static FillerAggregator()
    {
        fillers.Add(new MedianDeviationFiller());
        fillers.Add(new AverageDeviationFiller());
        fillers.Add(new ExpectedValueAndStdDeviationFiller());
        fillers.Add(new OutlierFiller());
    }

    public static void ApplyFillers(ParseResponse instance)
    {
        fillers.ForEach(filler => filler.FillResponse(instance));
    }
}