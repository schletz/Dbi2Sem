using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchulDbGenerator.Extensions;
using SchulDbGenerator.Model;
using SchulDbGenerator.Untis;

namespace SchulDbGenerator
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            await Task.CompletedTask;
            var options = new DbContextOptionsBuilder<SchuleContext>()
                .UseSqlite("DataSource=Schule.db")
                .Options;

            try
            {
                Untisdata data = await Untisdata.Load("Data", "2019-2020");
                using (SchuleContext db = new SchuleContext(options))
                {
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    db.SeedDatabase(data, 2019);
                }
            }
            catch (SchulDbException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e?.InnerException.Message);
                Console.Error.WriteLine(e.StackTrace);
                return 1;
            }
            return 0;
        }
    }
}