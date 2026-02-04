# DogApi 
Dette er en demo-applikation, der demonstrerer en REST API bygget i .NET 8, som kører i samspil med en PostgreSQL database via Docker Compose.

Projektet er designet til at være "Cloud Ready" med fokus på Container arkitektur

-.Net 8 (API)
-PostgreSQL (Database med persistens via Docker volumes)
-Docker & Docker Compose
-Npgsql (Database driver med Dependency injection)

Sådan startes systemet:

Der skal blot bruges en kommando for at starte både API'en og databasen

docker compose up --build
Systemet er klar, når du ser beskeden: database system is ready to accept connections.

Endpoints
1. Se alle hunde (GET)

Åbn din browser eller Postman/Thunder Client: http://localhost:8080/dogs
2. Tilføj en ny hund (POST)

Send en POST request til http://localhost:8080/dogs med følgende JSON body:
{
  "name": "Peter",
  "breed": "Magnum",
  "age": 4
}

Arkitektur

Projektet bruger en docker-compose.yml fil til at rejse to services:

1-api: Kører på port 8080.

2-db: Kører på port 5432 (intern Docker netværk) med en persistent volume, så data overlever genstart.