using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchneeDbGenerator
{
    public class MultiDbContext : DbContext
    {
        /// <summary>
        /// Liest das Adminpasswort von der Konsole und gibt je nach übergebenem DBMS die
        /// DbContextOptions zurück.
        /// </summary>
        public static DbContextOptions? GetConnectionInteractive(string dbms, string database)
        {
            Console.Clear();
            string adminPassword = dbms switch { "sqlserver" => "SqlServer2019", "oracle" => "oracle", _ => string.Empty };
            if (!string.IsNullOrEmpty(adminPassword))
            {
                Console.Write($"Admin Password for {dbms} database (ENTER bedeutet: verwende {adminPassword}): ");
                var line = Console.ReadLine();
                adminPassword = string.IsNullOrEmpty(line) ? adminPassword : line;
            }
            return dbms switch
            {
                "sqlserver" => GetSqlServerConnection(database, adminPassword),
                "oracle" => GetOracleConnection(database, "oracle", adminPassword),
                _ => GetSqliteConnection(database),
            };

        }
        private static DbContextOptions GetSqliteConnection(string database)
        {
            Console.WriteLine("*********************************************************");
            Console.WriteLine("Du kannst dich nun mit folgenden Daten verbinden:");
            Console.WriteLine($"Dateiname der SQLIte Datenbank: {database}.db");
            Console.WriteLine("*********************************************************");

            return new DbContextOptionsBuilder()
                .UseSqlite($"Data Source={database}.db")
                .Options;
        }

        private static DbContextOptions GetSqlServerConnection(string database, string saPassword = "SqlServer2019")
        {
            Console.WriteLine("**************************************************************************");
            Console.WriteLine("Du kannst dich nun mit folgenden Daten zur SQL Server Datenbank verbinden:");
            Console.WriteLine($"   Username:      sa");
            Console.WriteLine($"   Passwort:      {saPassword}");
            Console.WriteLine($"   Database Name: {database}");
            Console.WriteLine("**************************************************************************");

            return new DbContextOptionsBuilder()
                .UseSqlServer($"Server=127.0.0.1,1433;Initial Catalog={database};User Id=sa;Password={saPassword}")
                .Options;
        }
        private static DbContextOptions? GetOracleConnection(string username, string password, string systemPassword = "oracle")
        {
            try
            {
                var sysOpt = new DbContextOptionsBuilder()
                    .UseOracle($"User Id=System;Password={systemPassword};Data Source=localhost:1521/XEPDB1")
                    .Options;

                using (var sysDb = new MultiDbContext(sysOpt))
                {
                    try { sysDb.Database.ExecuteSqlRaw("DROP USER " + username + " CASCADE"); }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                    sysDb.Database.ExecuteSqlRaw("CREATE USER " + username + " IDENTIFIED BY " + password);
                    sysDb.Database.ExecuteSqlRaw("GRANT CONNECT, RESOURCE, CREATE VIEW TO " + username);
                    sysDb.Database.ExecuteSqlRaw("GRANT UNLIMITED TABLESPACE TO " + username);
                }
                Console.WriteLine("**********************************************************************");
                Console.WriteLine("Du kannst dich nun mit folgenden Daten zur Oracle Datenbank verbinden:");
                Console.WriteLine($"   Username:     {username}");
                Console.WriteLine($"   Passwort:     {password}");
                Console.WriteLine($"   Service Name: XEPDB1");
                Console.WriteLine("**********************************************************************");
                return new DbContextOptionsBuilder()
                    .UseOracle($"User Id={username};Password={password};Data Source=localhost:1521/XEPDB1")
                    .Options;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Fehler beim Löschen und neu Anlegen des Oracle Benutzers.");
                Console.Error.WriteLine("Fehlermeldung: " + e.Message);
                return null;
            }
        }

        public MultiDbContext(DbContextOptions opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Für Oracle alle Namen großschreiben, sonst sind sie Case Sensitive und brauchen
            // ein " bei den Abfragen.
            if (Database.IsOracle())
            {
                foreach (var entity in modelBuilder.Model.GetEntityTypes())
                {
                    var schema = entity.GetSchema();
                    var tableName = entity.GetTableName();
                    if (tableName is null) { continue; }
                    var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, schema);
                    foreach (var property in entity.GetProperties())
                    {
                        property.SetColumnName(property.GetColumnName(storeObjectIdentifier)?.ToUpper());
                    }
                    entity.SetTableName(tableName.ToUpper());
                }
            }
        }

    }
}
