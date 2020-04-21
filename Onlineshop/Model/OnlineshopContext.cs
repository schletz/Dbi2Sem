using Bogus;
using Bogus.Distributions.Gaussian;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Onlineshop.Model
{
    public class Kunde
    {
        public int KundeId { get; set; }
        public string Vorname { get; set; }
        public string Zuname { get; set; }
        public string Bundesland { get; set; }
    }

    public class Kategorie
    {
        public int KategorieId { get; set; }
        public string Name { get; set; }
    }

    public class Artikel
    {
        public int ArtikelId { get; set; }
        public string EAN { get; set; }
        public string Name { get; set; }
        public decimal Preis { get; set; }
        public Kategorie Kategorie { get; set; }
    }

    public class Bestellung
    {
        public int BestellungId { get; set; }
        public DateTime Datum { get; set; }
        public Kunde Kunde { get; set; }
        public List<Position> Positionen { get; set; }
    }

    public class Position
    {
        public int PositionId { get; set; }
        public Bestellung Bestellung { get; set; }
        public Artikel Artikel { get; set; }
        public int Menge { get; set; }
    }

    public class OnlineshopContext : DbContext
    {
        DbSet<Kunde> Kunden { get; set; }
        DbSet<Kategorie> Kategorien { get; set; }
        DbSet<Artikel> Artikel { get; set; }
        DbSet<Bestellung> Bestellungen { get; set; }
        DbSet<Position> Positionen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Shop.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artikel>().Property(a => a.Preis).HasColumnType("DECIMAL(9,4)");
        }

        public void Seed()
        {
            Randomizer.Seed = new Random(3858);
            Faker f = new Faker("de");

            var bundeslaender = new string[] { "N", "W", "B" };
            var kunden = new Faker<Kunde>()
                .RuleFor(k => k.Vorname, f=>f.Name.FirstName())
                .RuleFor(k => k.Zuname, f => f.Name.LastName())
                .RuleFor(k => k.Bundesland, f => f.Random.ListItem(bundeslaender))
                .Generate(16)
                .ToList();
            Kunden.AddRange(kunden);
            SaveChanges();

            var kategorien = new Faker<Kategorie>()
                .RuleFor(k => k.Name, f => f.Commerce.ProductMaterial())
                .Generate(4)
                .ToList();
            Kategorien.AddRange(kategorien);
            SaveChanges();

            var artikel = new Faker<Artikel>()
                .RuleFor(a => a.EAN, f => f.Commerce.Ean13())
                .RuleFor(a => a.Name, f => f.Commerce.ProductName())
                .RuleFor(a=>a.Preis, f=>Math.Round(Math.Min(226, f.Random.GaussianDecimal(200, 30)), 2))
                .RuleFor(a => a.Kategorie, f => f.Random.ListItem(kategorien))
                .Generate(16)
                .ToList();
            Artikel.AddRange(artikel);
            SaveChanges();

            var bestellungen = new Faker<Bestellung>()
                .RuleFor(b => b.Datum, f=> new DateTime(2020, 1, 1).AddSeconds(f.Random.Double(0, 20 * 86400)))
                .RuleFor(b => b.Kunde, f => f.Random.ListItem(kunden))
                .Generate(64);
            Bestellungen.AddRange(bestellungen);

            var positionen = new Faker<Position>()
                .RuleFor(p => p.Bestellung, f => f.Random.ListItem(bestellungen))
                .RuleFor(p => p.Artikel, f => f.Random.ListItem(artikel))
                .RuleFor(p => p.Menge, f => f.Random.Int(1, 10))
                .Generate(128);
            Positionen.AddRange(positionen);
            SaveChanges();
        }
    }

}
