using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchulDb.Model;
using SchulDb.Untis;

namespace SchulDbGenerator
{
    class Program
    {
        static DbContextOptions<SchuleContext> GetOptions(int dbType)
        {
            var builder = new DbContextOptionsBuilder<SchuleContext>();

            if (dbType == 1)
            {
                Console.Write("Dateiname? Hinweis: Relative Pfade (..) sind möglich. Default: Schule.db ");
                string dbName = Console.ReadLine();
                dbName = string.IsNullOrEmpty(dbName) ? "Schule.db" : dbName;
                builder.UseSqlite($"DataSource={dbName}");
            }
            else if (dbType == 2)
            {
                Console.Write("Wie soll die Datenbank heißen? Default: Schule ");
                string dbName = Console.ReadLine();
                dbName = string.IsNullOrEmpty(dbName) ? "Schule" : dbName;
                builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;" +
                                $"AttachDBFilename={System.Environment.CurrentDirectory}\\{dbName}.mdf;" +
                                $"Database={dbName};" +
                                $"Trusted_Connection=True;MultipleActiveResultSets=true");
            }
            else if (dbType == 5)
            {
                Console.Write("Gib das sa Passwort ein, das beim Erstellen des Containers verwendet wurde (ENTER bedeutet: verwende SqlServer2019). ");
                string password = Console.ReadLine();
                password = string.IsNullOrEmpty(password) ? "SqlServer2019" : password;
                
                Console.Write("Wie soll die Datenbank heißen (ENTER bedeutet: verwende SchulDb)? ");
                string dbName = Console.ReadLine();
                dbName = string.IsNullOrEmpty(dbName) ? "SchulDb" : dbName;
                builder.UseSqlServer($"Server=127.0.0.1,1433;Initial Catalog={dbName};User Id=sa;Password={password}");
            }
            else if (dbType == 3 || dbType == 4)
            {
                Console.Write("Gib das system Passwort ein, das beim Erstellen des Containers verwendet wurde (ENTER bedeutet: verwende oracle). ");
                string password = Console.ReadLine();   
                password = string.IsNullOrEmpty(password) ? "oracle" : password;

                Console.Write("Wie soll der Benutzer heißen (ENTER bedeutet: verwende Schule)? ");
                string dbName = Console.ReadLine();
                dbName = string.IsNullOrEmpty(dbName) ? "Schule" : dbName;
                // Oracle 19 und 21 arbeiten mit pluggable Databases. Diese können mit
                // nach einem Login als system mit derm Service Name XE und folgendem Statement herausgefunden werden:
                // SELECT name FROM v$pdbs;
                var serviceName = dbType == 3 ? "orcl" : "XEPDB1";
                builder.UseOracle($"User Id={dbName};Password=oracle;Data Source=localhost:1521/{serviceName}");

                var oracleSystemOptions = new DbContextOptionsBuilder<SchuleContext>()
                    .UseOracle($"User Id=System;Password={password};Data Source=localhost:1521/{serviceName}")
                    .Options;

                Console.WriteLine($"Lege Benutzer {dbName} mit Passwort oracle an...");
                try
                {
                    using (SchuleContext db = new SchuleContext(oracleSystemOptions))
                    {
                        // Warning wegen möglicher SQL Injections. Da dies aber kein Produktionscode
                        // ist, wird sie deaktiviert. Außerdem funktioniert keine andere Variante
                        // (OracleParameter, Interpolated String, ...).
#pragma warning disable EF1000
                        try { db.Database.ExecuteSqlRaw("DROP USER " + dbName + " CASCADE"); }
                        catch { }
                        db.Database.ExecuteSqlRaw("CREATE USER " + dbName + " IDENTIFIED BY oracle");
                        db.Database.ExecuteSqlRaw("GRANT CONNECT, RESOURCE, CREATE VIEW TO " + dbName);
                        db.Database.ExecuteSqlRaw("GRANT UNLIMITED TABLESPACE TO " + dbName);
                    }
                    Console.WriteLine("*********************************************************");
                    Console.WriteLine("Du kannst dich nun mit folgenden Daten verbinden:");
                    Console.WriteLine($"   Username:     {dbName}");
                    Console.WriteLine($"   Passwort:     oracle");
                    Console.WriteLine($"   Service Name: {serviceName}");
                    Console.WriteLine("*********************************************************");
                }
                catch (Exception e)
                {
                    throw new SchulDb.SchulDbException("Fehler beim Löschen und neu Anlegen des Oracle Benutzers." + Environment.NewLine
                        + "Fehlermeldung: " + e.Message + Environment.NewLine
                        + "Mögliche Ursachen: Der Benutzer ist gerade aktiv oder die VM läuft nicht.");
                }
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
                int dbType = args.Length > 0 && args[0] == "sqlserver" ? 5
                    : args.Length > 0 && args[0] == "oracle" ? 4
                    : 0;
                while (dbType == 0)
                {
                    Console.Write("Welche Datenbank soll erstellt werden? [1]: SQLite (Default)   [2]: LocalDb   [3]: Oracle 12 (VM)   [4]: Oracle 19 XE oder 21 XE   [5] SQL Server Docker Image   ");
                    if (int.TryParse(Console.ReadLine(), out var val) && val <= 5 && val >= 1)
                    {
                        dbType = val;
                    }
                }
                var options = GetOptions(dbType);
                Untisdata data = await Untisdata.Load("Data");
                using (SchuleContext db = new SchuleContext(options))
                {
                    Console.WriteLine("Lösche die alte Datenbank...");
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    Console.WriteLine("Schreibe Musterdaten...");
                    db.SeedDatabase(data, 2021);
                    Console.WriteLine("FERTIG!");
                }
            }
            catch (Exception e)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e?.InnerException?.Message);
                Console.ForegroundColor = color;
                return 1;
            }
            return 0;
        }
    }
}