using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Fillers;

public interface IResponseFiller
{
    public void FillResponse(ParseResponse response);
}