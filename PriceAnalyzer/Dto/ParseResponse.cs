using AvitoParser;

namespace PriceAnalyzer.Dto;

public record ParseResponse(double AveragePrice, IList<Advertisement> Advertisements);