using System;

namespace Lieferservice
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
            var options = MultiDbContext.GetConnectionInteractive(dbms: args[0].ToLower(), database: "Lieferservice");
            if (options is null) { return; }
            using (LieferserviceContext db = new LieferserviceContext(options))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.Seed();
            }
        }
    }
}
