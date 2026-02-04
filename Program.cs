using Npgsql;

var builder = WebApplication.CreateBuilder(args);


var connectionString = "Host=db;Username=postgres;Password=hemmeligt;Database=hundedb";
builder.Services.AddNpgsqlDataSource(connectionString);

var app = builder.Build();


app.MapGet("/dogs", async (NpgsqlDataSource db) =>
{
    using var conn = await db.OpenConnectionAsync();
    using var cmd = new NpgsqlCommand("SELECT id, name, breed, age FROM dogs", conn);
    using var reader = await cmd.ExecuteReaderAsync();

    var dogs = new List<Dog>();
    while (await reader.ReadAsync())
    {
        dogs.Add(new Dog(
            reader.GetInt32(0), 
            reader.GetString(1),
            reader.GetString(2),
            reader.GetInt32(3)   
        ));
    }
    return dogs;
});


app.MapPost("/dogs", async (CreateDogDto nyHund, NpgsqlDataSource db) =>
{
    using var conn = await db.OpenConnectionAsync();

    using var cmd = new NpgsqlCommand("INSERT INTO dogs (name, breed, age) VALUES ($1, $2, $3) RETURNING id", conn);
    
    
    cmd.Parameters.AddWithValue(nyHund.Name);
    cmd.Parameters.AddWithValue(nyHund.Breed);
    cmd.Parameters.AddWithValue(nyHund.Age);
    
    var newId = (int)await cmd.ExecuteScalarAsync();

    return Results.Created($"/dogs/{newId}", new Dog(newId, nyHund.Name, nyHund.Breed, nyHund.Age));
});


app.MapDelete("/dogs/{id}", async (int id, NpgsqlDataSource db) =>
{
    using var conn = await db.OpenConnectionAsync();
    using var cmd = new NpgsqlCommand("DELETE FROM dogs WHERE id = $1", conn);
    cmd.Parameters.AddWithValue(id);
    
    var rowsAffected = await cmd.ExecuteNonQueryAsync();

    if (rowsAffected == 0)
    {
        return Results.NotFound($"Kunne ikke finde hund med ID {id}");
    }

    return Results.NoContent();
});

app.Run();

public record Dog(int Id, string Name, string Breed, int Age);
public record CreateDogDto(string Name, string Breed, int Age);