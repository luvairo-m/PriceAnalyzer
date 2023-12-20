using AvitoParser;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var g = ConfigurationHelper.DefaultHeaders;

app.MapGet("/", () => "Hello World!");

app.Run();