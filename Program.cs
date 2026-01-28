var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var dogs = new[]
{
    new { Name = "Ninjago", Breed = "Chiwawa", Age = 3 },
    new { Name = "Rose", Breed = "Golden Retriever", Age = 4 },
    new { Name = "Sia", Breed = "Labrador", Age = 2 }
};

app.MapGet("/dogs", () => dogs);
app.MapGet("/", () => "Dette er DCR! Gå til /dogs for at se hundene");

app.Run();
