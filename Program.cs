/*
--->So findest du die Datenbank<---
Öffne den Projekt-Explorer in Visual Studio.
Navigiere zum Ordner bin/Debug/netX.X/.
Suche die Datei sample.db. 
-------------------------------------------------------------------------------------------------------------------------------------------------------
Datenbank an einem benutzerdefinierten Ort speichern
Du kannst den Speicherort explizit angeben, indem du den Pfad im Connection-String oder bei SQLiteConnection.CreateFile änderst. 

Beispiel:
string connectionString = @"Data Source=C:\MeineDatenbanken\sample.db;Version=3;";
SQLiteConnection.CreateFile(@"C:\MeineDatenbanken\sample.db");
Hier wird die Datenbank unter C:\MeineDatenbanken\ erstellt.
-------------------------------------------------------------------------------------------------------------------------------------------------------
Falls du eine Umgebung wie einen Desktop-Benutzerordner oder ein temporäres Verzeichnis nutzen möchtest, kannst du den Pfad dynamisch setzen:

string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sample.db");
string connectionString = $"Data Source={dbPath};Version=3;";
SQLiteConnection.CreateFile(dbPath);
Das speichert die Datenbank im Dokumentenordner des aktuellen Benutzers.
-------------------------------------------------------------------------------------------------------------------------------------------------------
 */

using System;
using System.Data.SQLite;


namespace SQLiteApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=sample.db;Version=3;";

            // 1. Datenbank erstellen (falls nicht vorhanden)
            CreateDatabase(connectionString);

            // 2. Tabelle erstellen
            CreateTable(connectionString);

            // 3. Daten hinzufügen
            InsertData(connectionString, "John Wick", 28);
            InsertData(connectionString, "Sailer Monn", 34);

            // 4. Daten auslesen
            ReadData(connectionString);

            Console.ReadLine();
        }
        //-------------------------------------------------------------------------------------------------------CreateDatabase
        static void CreateDatabase(string connectionString)
        {
            SQLiteConnection.CreateFile("sample.db");
            Console.WriteLine("Datenbank erstellt: sample.db");
        }

        static void CreateTable(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Age INTEGER NOT NULL
                );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Tabelle 'Users' erstellt.");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------InsertData
        static void InsertData(string connectionString, string name, int age)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age);";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Daten hinzugefügt: {name}, {age} Jahre alt.");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------ReadData
        static void ReadData(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Users;";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Daten in der Tabelle 'Users':");
                        while (reader.Read())
                        {
                            Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, Alter: {reader["Age"]}");
                        }
                    }
                }
            }
        }

    }
}
