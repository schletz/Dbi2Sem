using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AufsichtPlanner
{
    public class OracleSqlserverContext : DbContext
    {
        public static (bool success, DbContextOptions options) GetSqlServerConnection(string database, string saPassword = "SqlServer2019")
        {
            Console.WriteLine("*********************************************************");
            Console.WriteLine("Du kannst dich nun mit folgenden Daten verbinden:");
            Console.WriteLine($"   Username:      sa");
            Console.WriteLine($"   Passwort:      {saPassword}");
            Console.WriteLine($"   Database Name: {database}");
            Console.WriteLine("*********************************************************");

            return (true, new DbContextOptionsBuilder()
                .UseSqlServer($"Server=127.0.0.1,1433;Initial Catalog={database};User Id=sa;Password={saPassword}")
                .Options);
        }
        public static (bool success, DbContextOptions options) GetOracleConnection(string username, string password, string systemPassword = "oracle")
        {
            try
            {
                var sysOpt = new DbContextOptionsBuilder()
                    .UseOracle($"User Id=System;Password={systemPassword};Data Source=localhost:1521/XEPDB1")
                    .Options;

                using (var sysDb = new OracleSqlserverContext(sysOpt))
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
                Console.WriteLine("*********************************************************");
                Console.WriteLine("Du kannst dich nun mit folgenden Daten verbinden:");
                Console.WriteLine($"   Username:     {username}");
                Console.WriteLine($"   Passwort:     {password}");
                Console.WriteLine($"   Service Name: XEPDB1");
                Console.WriteLine("*********************************************************");
                return (true, new DbContextOptionsBuilder()
                    .UseOracle($"User Id={username};Password={password};Data Source=localhost:1521/XEPDB1")
                    .Options);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Fehler beim Löschen und neu Anlegen des Oracle Benutzers.");
                Console.Error.WriteLine("Fehlermeldung: " + e.Message);
                return (false, new DbContextOptionsBuilder().Options);
            }
        }

        public OracleSqlserverContext(DbContextOptions opt) : base(opt)
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
