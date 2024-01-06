using AvitoParser;

namespace PriceAnalyzer.Dto;

public class ParseResponse
{
    public double MedianPrice { get; set; }
    public double AveragePrice { get; set; }
    public double ExpectedValue { get; set; }
    public double StandardDeviation { get; set; }
    public List<Advertisement> Advertisements { get; set; }
}