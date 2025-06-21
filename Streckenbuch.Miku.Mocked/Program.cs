using Streckenbuch.Miku.Mocked;
using Streckenbuch.Miku.Models.Fahrten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

Data data = new Data();

app.MapGet("/ws/fahrt/{trainNumber:int}", (int trainNumber) => data.GetNextData()).WithName("GetTrainData");
app.MapGet("/reset", () => data.LoadData());

app.Run();