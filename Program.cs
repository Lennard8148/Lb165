using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Lb165
{



    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "mongodb://localhost:27017";
            var databaseName = "Blogsystem";
            var collectionName = "posts";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            while (true)
            {
                Console.WriteLine("Bitte wählen Sie eine Aktion aus:");
                Console.WriteLine("1. Dokument hinzufügen");
                Console.WriteLine("2. Dokument lesen");
                Console.WriteLine("3. Dokument aktualisieren");
                Console.WriteLine("4. Dokument löschen");
                Console.WriteLine("5. Beenden");
                Console.Write("Auswahl: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await InsertDocumentAsync(collection);
                        break;
                    case "2":
                        await ReadDocumentAsync(collection);
                        break;
                    case "3":
                        await UpdateDocumentAsync(collection);
                        break;
                    case "4":
                        await DeleteDocumentAsync(collection);
                        break;
                    case "5":
                        Console.WriteLine("Programm wird beendet.");
                        return;
                    default:
                        Console.WriteLine("Ungültige Auswahl. Bitte geben Sie eine Zahl zwischen 1 und 5 ein.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static async Task InsertDocumentAsync(IMongoCollection<BsonDocument> collection)
        {
            Console.WriteLine("Neues Dokument hinzufügen:");
            Console.Write("Titel: ");
            var title = Console.ReadLine();

            Console.Write("Inhalt: ");
            var content = Console.ReadLine();

            Console.Write("Autor: ");
            var author = Console.ReadLine();

            Console.Write("Nummer: ");
            var number = Console.ReadLine();

            var document = new BsonDocument
        {
            { "title", title },
            { "content", content },
            { "author", author },
            { "number", number }
        };

            await collection.InsertOneAsync(document);
            Console.WriteLine("Dokument erfolgreich hinzugefügt.");
        }

        static async Task ReadDocumentAsync(IMongoCollection<BsonDocument> collection)
        {
            Console.Write("Autor des Dokuments zum Lesen eingeben: ");
            var author = Console.ReadLine();

            var filter = Builders<BsonDocument>.Filter.Eq("author", author);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                Console.WriteLine("Gefundenes Dokument:");
                Console.WriteLine(result.ToJson());
            }
            else
            {
                Console.WriteLine($"Kein Dokument gefunden für Autor '{author}'.");
            }
        }

        static async Task UpdateDocumentAsync(IMongoCollection<BsonDocument> collection)
        {
            Console.Write("Autor des zu aktualisierenden Dokuments eingeben: ");
            var author = Console.ReadLine();

            Console.Write("Neuer Inhalt: ");
            var newContent = Console.ReadLine();

            var updateFilter = Builders<BsonDocument>.Filter.Eq("author", author);
            var updateDefinition = Builders<BsonDocument>.Update.Set("content", newContent);

            var result = await collection.UpdateOneAsync(updateFilter, updateDefinition);

            if (result.ModifiedCount > 0)
            {
                Console.WriteLine($"Dokument für Autor '{author}' erfolgreich aktualisiert.");
            }
            else
            {
                Console.WriteLine($"Kein Dokument gefunden für Autor '{author}'. Aktualisierung fehlgeschlagen.");
            }
        }

        static async Task DeleteDocumentAsync(IMongoCollection<BsonDocument> collection)
        {
            Console.Write("Autor des zu löschenden Dokuments eingeben: ");
            var author = Console.ReadLine();

            var deleteFilter = Builders<BsonDocument>.Filter.Eq("author", author);
            var result = await collection.DeleteOneAsync(deleteFilter);

            if (result.DeletedCount > 0)
            {
                Console.WriteLine($"Dokument für Autor '{author}' erfolgreich gelöscht.");
            }
            else
            {
                Console.WriteLine($"Kein Dokument gefunden für Autor '{author}'. Löschen fehlgeschlagen.");
            }
        }
    }


}