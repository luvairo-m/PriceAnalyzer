using PriceAnalyzer;

var builder = WebApplication.CreateBuilder(args);

// Typed httpClient registration:
builder.Services.AddHttpClient<ParsingClient>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();