using Npgsql;

var builder = WebApplication.CreateBuilder(args);


var connectionString = "Host=db;Username=postgres;Password=hemmeligt;Database=hundedb";
builder.Services.AddNpgsqlDataSource(connectionString);

var app = builder.Build();


app.MapGet("/dogs", async (NpgsqlDataSource db) =>
{
    
    using var conn = await db.OpenConnectionAsync();
    
    using var cmd = new NpgsqlCommand("SELECT name, breed, age FROM dogs", conn);
    using var reader = await cmd.ExecuteReaderAsync();

    var dogs = new List<Dog>();
    while (await reader.ReadAsync())
    {
        dogs.Add(new Dog(
            reader.GetString(0), 
            reader.GetString(1), 
            reader.GetInt32(2)   
        ));
    }
    return dogs;
});


app.MapPost("/dogs", async (Dog hund, NpgsqlDataSource db) =>
{
    using var conn = await db.OpenConnectionAsync();
    
    using var cmd = new NpgsqlCommand("INSERT INTO dogs (name, breed, age) VALUES ($1, $2, $3)", conn);
    cmd.Parameters.AddWithValue(hund.Name);
    cmd.Parameters.AddWithValue(hund.Breed);
    cmd.Parameters.AddWithValue(hund.Age);
    
    await cmd.ExecuteNonQueryAsync();

    return Results.Created($"/dogs/{hund.Name}", hund);
});

app.Run();


public record Dog(string Name, string Breed, int Age);