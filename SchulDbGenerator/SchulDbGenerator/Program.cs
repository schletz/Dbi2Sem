using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchulDb.Model;
using SchulDb.Untis;

namespace SchulDbGenerator
{
    class Program
    {
        static DbContextOptions<SchuleContext> GetOptions()
        {
            var builder = new DbContextOptionsBuilder<SchuleContext>();
            Console.Write("Welche Datenbank soll erstellt werden? [1]: SQLite (Default)   [2]: LocalDb ");
            string dbType = Console.ReadLine();
            dbType = string.IsNullOrEmpty(dbType) ? "1" : dbType;

            if (dbType == "1")
            {
                Console.Write("Dateiname? Hinweis: Relative Pfade (..) sind möglich. Default: Schule.db ");
                string dbName = Console.ReadLine();
                dbName = string.IsNullOrEmpty(dbName) ? "Schule.db" : dbName;
                builder.UseSqlite($"DataSource={dbName}");
            }
            else if (dbType == "2")
            {
                Console.Write("Wie soll die Datenbank heißen? Default: Schule ");
                string dbName = Console.ReadLine();
                dbName = string.IsNullOrEmpty(dbName) ? "Schule" : dbName;
                builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;" +
                                $"AttachDBFilename={System.Environment.CurrentDirectory}\\{dbName}.mdf;" +
                                $"Database={dbName};" +
                                $"Trusted_Connection=True;MultipleActiveResultSets=true");
            }
            else
            {
                throw new SchulDb.SchulDbException("Ungültige Eingabe.");
            }
            return builder.Options;
        }
        static async Task<int> Main(string[] args)
        {
            try
            {
                var options = GetOptions();
                Untisdata data = await Untisdata.Load("Data", "2019-2020");
                using (SchuleContext db = new SchuleContext(options))
                {
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    db.SeedDatabase(data, 2019);
                }
            }
            catch (SchulDb.SchulDbException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e?.InnerException.Message);
                Console.Error.WriteLine(e.StackTrace);
                return 1;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e?.InnerException?.Message);
                Console.Error.WriteLine(e.StackTrace);
                return 2;
            }
            return 0;
        }
    }
}