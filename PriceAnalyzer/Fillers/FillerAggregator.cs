using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public static class FillerAggregator
{
    private static readonly List<IResponseFiller> fillers = new();

    static FillerAggregator()
    {
        fillers.Add(new MedianDeviationFiller());
        fillers.Add(new ExpectedValueAndStdDeviationFiller());
        fillers.Add(new OutlierFiller());
        fillers.Add(new ModeFiller());
    }

    public static void ApplyFillers(ParseResponse instance)
    {
        fillers.ForEach(filler => filler.FillResponse(instance));
    }
}