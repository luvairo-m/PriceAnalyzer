namespace AvitoParser;

public interface IParser<T>
{
    public Task<IList<T>> GetParsedData(string url, int amount);
}