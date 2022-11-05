using Lieferservice;
using System;

namespace Onlineshop
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Missing args.");
                Console.Error.WriteLine("Usage: dotnet run -- (sqlserver|oracle|sqlite)");
                return;
            }
            var options = MultiDbContext.GetConnectionInteractive(dbms: args[0].ToLower(), database: "Onlineshop");
            if (options is null) { return; }

            using (OnlineshopContext db = new OnlineshopContext(options))
            {
                db.Database.EnsureDeleted();
                if (db.Database.EnsureCreated()) { db.Seed(); }
            }
        }
    }
}
