using AvitoParser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

var factory = app.Services.GetRequiredService<IHttpClientFactory>();
var parser = new Parser(factory.CreateClient());
var adverts = await
    parser.GetAdvertisements("https://www.avito.ru/kurganskaya_oblast/velosipedy?cd=1&f=ASgBAgICAUT~vA2i0jQ&p=6",
        25);

foreach (var card in adverts)
{
    Console.WriteLine("Card title: " + card.Title);
    Console.WriteLine("Card price: " + card.Price);
    Console.WriteLine("Card city: " + card.City);
    Console.WriteLine("Card date: " + card.PublicationDate);
    Console.WriteLine("Card url: " + card.Url);
}

Console.Write(adverts.Count);

app.Run();