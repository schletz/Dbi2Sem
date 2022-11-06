using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WienerlinienDb.Csv;

namespace WienerlinienDb.Model
{
    public class WienerlinienContext : MultiDbContext
    {
        public DbSet<Haltestelle> Haltestellen => Set<Haltestelle>();
        public DbSet<Linie> Linien => Set<Linie>();
        public DbSet<Steig> Steige => Set<Steig>();
        public DbSet<Fahrt> Fahrten => Set<Fahrt>();

        public WienerlinienContext(DbContextOptions opt) : base(opt)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Haltestelle>().ToTable("Haltestelle");
            modelBuilder.Entity<Linie>().ToTable("Linie");
            modelBuilder.Entity<Steig>().ToTable("Steig");


            modelBuilder.Entity<Haltestelle>().Property(h => h.HaltestelleId).ValueGeneratedNever();
            modelBuilder.Entity<Haltestelle>().Property(h => h.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Haltestelle>().Property(h => h.Gemeinde).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Haltestelle>().Property(h => h.Wgs84_Lat).HasColumnType("DECIMAL(18, 15)");
            modelBuilder.Entity<Haltestelle>().Property(h => h.Wgs84_Lon).HasColumnType("DECIMAL(18, 15)");

            modelBuilder.Entity<Linie>().Property(l => l.LinieId).ValueGeneratedNever();
            modelBuilder.Entity<Linie>().Property(l => l.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Linie>().Property(l => l.Reihenfolge).IsRequired();
            modelBuilder.Entity<Linie>().Property(l => l.Verkehrsmittel).IsRequired().HasMaxLength(255);

            modelBuilder.Entity<Steig>().Property(s => s.SteigId).ValueGeneratedNever();
            modelBuilder.Entity<Steig>().HasOne(s => s.Linie).WithMany(l => l.Steige).IsRequired();
            modelBuilder.Entity<Steig>().HasOne(s => s.Haltestelle).WithMany(h => h.Steige).IsRequired();
            modelBuilder.Entity<Steig>().Property(s => s.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Steig>().Property(s => s.Richtung).IsRequired().HasMaxLength(1);
            modelBuilder.Entity<Steig>().Property(s => s.Wgs84_Lat).HasColumnType("DECIMAL(18, 15)");
            modelBuilder.Entity<Steig>().Property(s => s.Wgs84_Lon).HasColumnType("DECIMAL(18, 15)");

            modelBuilder.Entity<Fahrt>().HasOne(f => f.Einstieg).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Fahrt>().HasOne(f => f.Ausstieg).WithMany().OnDelete(DeleteBehavior.Restrict);
        }

        public void Seed()
        {
            Randomizer.Seed = new Random(3858);
            Faker f = new Faker("de");

            var linienCsv = f.Random.ListItems(Datareader.ReadFile<Datareader.Linie>("Csv/wienerlinien-ogd-linien.csv"), 10);
            var selectedLinien = new HashSet<int>(linienCsv.Select(l => l.Linien_Id));
            var steigeCsv = Datareader.ReadFile<Datareader.Steig>("Csv/wienerlinien-ogd-steige.csv")
                .Where(s => selectedLinien.Contains(s.Fk_Linien_Id));
            var selectedHaltestellen = new HashSet<int>(steigeCsv.Select(s => s.Fk_Haltestellen_Id));
            var haltestellenCsv = Datareader.ReadFile<Datareader.Haltestelle>("Csv/wienerlinien-ogd-haltestellen.csv")
                .Where(h => selectedHaltestellen.Contains(h.Haltestellen_Id));

            decimal minLon = haltestellenCsv.Where(h => h.Gemeinde == "Wien").Min(h => h.Wgs84_Lon) ?? 0;
            decimal maxLon = haltestellenCsv.Where(h => h.Gemeinde == "Wien").Max(h => h.Wgs84_Lon) + 1e-6M ?? 0;

            Linien.AddRange(linienCsv.Select(l => new Linie
            {
                LinieId = l.Linien_Id,
                Name = l.Bezeichnung,
                Reihenfolge = l.Reihenfolge,
                Verkehrsmittel = l.Verkehrsmittel
            }));
            SaveChanges();

            Haltestellen.AddRange(haltestellenCsv
                .Select(h => new Haltestelle
                {
                    HaltestelleId = h.Haltestellen_Id,
                    Name = h.Name,
                    Gemeinde = h.Gemeinde,
                    Gemeinde_Id = h.Gemeinde_Id,
                    Gemeindebezirk = h.Gemeinde == "Wien" && h.Wgs84_Lon != null ? 10 + (int)((h.Wgs84_Lon - minLon) * 4 / (maxLon - minLon)) : (int?)null,
                    Wgs84_Lat = h.Wgs84_Lat,
                    Wgs84_Lon = h.Wgs84_Lon
                }));
            SaveChanges();

            Steige.AddRange(steigeCsv.Select(s => new Steig
            {
                SteigId = s.Steig_Id,
                Linie = Linien.Find(s.Fk_Linien_Id)!,
                Haltestelle = Haltestellen.Find(s.Fk_Haltestellen_Id)!,
                Name = s.Name,
                Richtung = s.Richtung,
                Reihenfolge = s.Reihenfolge,
                Wgs84_Lat = s.Steig_Wgs84_Lat,
                Wgs84_Lon = s.Steig_Wgs84_Lon
            }));
            SaveChanges();

            var haltestellenDb = Haltestellen.ToList();
            var fahrtFaker = new Faker<Fahrt>().Rules((fkr, fa) =>
            {
                fa.Fahrtantritt = fkr.Date.Between(new DateTime(2020, 4, 6), new DateTime(2020, 4, 13))
                    .Date
                    .AddSeconds(fkr.Random.Int(0, 86400 - 1));
                fa.Einstieg = fkr.Random.ListItem(haltestellenDb);
                fa.Ausstieg = fkr.Random.ListItem(haltestellenDb);
            });
            Fahrten.AddRange(fahrtFaker.Generate(64));
            SaveChanges();
        }
    }
}
