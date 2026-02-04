-- Opret en tabel til hundene
CREATE TABLE Dogs (
    Id SERIAL PRIMARY KEY,
    Name TEXT,
    Breed TEXT,
    Age INT
);

-- Indsæt nogle hunde (i stedet for Ninjago listen)
INSERT INTO Dogs (Name, Breed, Age) VALUES ('Batman', 'Bulldog', 5);
INSERT INTO Dogs (Name, Breed, Age) VALUES ('Wonder Woman', 'Puddel', 2);
INSERT INTO Dogs (Name, Breed, Age) VALUES ('Superman', 'Gravhund', 8);
INSERT INTO Dogs (Name, Breed, Age) VALUES ('Hulk', 'Monster', 10);

-- Vis mig hvad der er i tabellen nu
SELECT * FROM Dogs;