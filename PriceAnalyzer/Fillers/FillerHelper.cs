namespace PriceAnalyzer.Fillers;

public static class FillerHelper
{
    public static int GetDeviationPercent(double basis, double value)
    {
        var deviation = (basis - value) / value * 100;
        return (int)Math.Round(deviation);
    }
}