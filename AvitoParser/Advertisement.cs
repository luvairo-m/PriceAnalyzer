namespace AvitoParser;

public record Advertisement(
    string Title,
    string Url,
    string City,
    string Region,
    string Location,
    int Price,
    DateTime PublicationDate)
{
    public double PriceDeviation { get; set; }
}