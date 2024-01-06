using AvitoParser;

namespace PriceAnalyzer.Dto;

public record ParseResponse(double MedianPrice, List<Advertisement> Advertisements);