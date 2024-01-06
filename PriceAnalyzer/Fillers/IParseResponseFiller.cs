using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public interface IParseResponseFiller
{
    public void FillResponse(ParseResponse response);
}