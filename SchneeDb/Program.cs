using SchneeDbGenerator;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Missing args.");
            Console.Error.WriteLine("Usage: dotnet run -- (sqlserver|oracle|sqlite)");
            return;
        }
        var options = MultiDbContext.GetConnectionInteractive(dbms: args[0].ToLower(), database: "Schnee");
        if (options is null) { return; }
        using var db = new SchneeContext(options);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        Console.WriteLine("Befülle die Datenbank. Das kann etwas dauern...");
        db.Seed();
        Console.WriteLine("FERTIG!");
    }
}