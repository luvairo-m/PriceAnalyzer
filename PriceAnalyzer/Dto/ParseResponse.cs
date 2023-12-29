using AvitoParser;

namespace PriceAnalyzer.Dto;

public record ParseResponse(double MedianPrice, IList<Advertisement> Advertisements);