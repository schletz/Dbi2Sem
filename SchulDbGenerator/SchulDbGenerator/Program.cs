using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchulDb.Model;
using SchulDb.Untis;

namespace SchulDbGenerator
{
    enum DbType { Sqlite, Localdb }
    class Program
    {
        // ANPASSEN DER EINSTELLUNGEN
        static readonly DbType _dbtype = DbType.Localdb;
        static readonly string _dbName = "Schule";

        static async Task<int> Main(string[] args)
        {
            try
            {
                var options = new DbContextOptionsBuilder<SchuleContext>()
                        .UseSqlite($"DataSource={_dbName}.db")
                        .Options;

                if (_dbtype == DbType.Localdb)
                {
                    options = new DbContextOptionsBuilder<SchuleContext>()
                            .UseSqlServer($"Server=(localdb)\\mssqllocaldb;" +
                                $"AttachDBFilename={System.Environment.CurrentDirectory}\\{_dbName}.mdf;" +
                                $"Database={_dbName};" +
                                $"Trusted_Connection=True;MultipleActiveResultSets=true")
                            .Options;
                }

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
            }
            return 0;
        }
    }
}