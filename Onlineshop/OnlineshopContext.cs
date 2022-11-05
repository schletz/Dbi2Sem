using Bogus;
using Bogus.Distributions.Gaussian;
using Lieferservice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Onlineshop
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Kunde
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KundeId { get; set; }
        public string Vorname { get; set; }
        public string Zuname { get; set; }
        public string Bundesland { get; set; }
    }

    public class Kategorie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KategorieId { get; set; }
        public string Name { get; set; }
    }

    public class Artikel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArtikelId { get; set; }
        public string EAN { get; set; }
        public string Name { get; set; }
        public decimal Preis { get; set; }
        public Kategorie Kategorie { get; set; }
    }

    public class Bestellung
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BestellungId { get; set; }
        public DateTime Datum { get; set; }
        public Kunde Kunde { get; set; }
        public List<Position> Positionen { get; set; }
    }

    public class Position
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PositionId { get; set; }
        public Bestellung Bestellung { get; set; }
        public Artikel Artikel { get; set; }
        public int Menge { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class OnlineshopContext : MultiDbContext
    {
        DbSet<Kunde> Kunden => Set<Kunde>();
        DbSet<Kategorie> Kategorien => Set<Kategorie>();
        DbSet<Artikel> Artikel => Set<Artikel>();
        DbSet<Bestellung> Bestellungen => Set<Bestellung>();
        DbSet<Position> Positionen => Set<Position>();

        public OnlineshopContext(DbContextOptions db) : base(db)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Artikel>().Property(a => a.Preis).HasColumnType("DECIMAL(9,4)");
        }

        public void Seed()
        {
            Randomizer.Seed = new Random(3858);
            Faker f = new Faker("de");

            var bundeslaender = new string[] { "N", "W", "B" };
            int rownr = 1;
            var kunden = new Faker<Kunde>()
                .RuleFor(k => k.KundeId, f => rownr++)
                .RuleFor(k => k.Vorname, f => f.Name.FirstName())
                .RuleFor(k => k.Zuname, f => f.Name.LastName())
                .RuleFor(k => k.Bundesland, f => f.Random.ListItem(bundeslaender))
                .Generate(16)
                .ToList();
            Kunden.AddRange(kunden);
            SaveChanges();

            rownr = 1;
            var kategorien = new Faker<Kategorie>()
                .RuleFor(k => k.KategorieId, f => rownr++)
                .RuleFor(k => k.Name, f => f.Commerce.ProductMaterial())
                .Generate(4)
                .ToList();
            Kategorien.AddRange(kategorien);
            SaveChanges();

            rownr = 1;
            var artikel = new Faker<Artikel>()
                .RuleFor(a => a.ArtikelId, f => rownr++)
                .RuleFor(a => a.EAN, f => f.Commerce.Ean13())
                .RuleFor(a => a.Name, f => f.Commerce.ProductName())
                .RuleFor(a => a.Preis, f => Math.Round(Math.Min(226, f.Random.GaussianDecimal(200, 30)), 2))
                .RuleFor(a => a.Kategorie, f => f.Random.ListItem(kategorien))
                .Generate(16)
                .ToList();
            Artikel.AddRange(artikel);
            SaveChanges();

            rownr = 1;
            var bestellungen = new Faker<Bestellung>()
                .RuleFor(b => b.BestellungId, f => rownr++)
                .RuleFor(b => b.Datum, f => new DateTime(2020, 1, 1).AddSeconds(f.Random.Double(0, 20 * 86400)))
                .RuleFor(b => b.Kunde, f => f.Random.ListItem(kunden))
                .Generate(64);
            Bestellungen.AddRange(bestellungen);

            rownr = 1;
            var positionen = new Faker<Position>()
                .RuleFor(p => p.PositionId, f => rownr++)
                .RuleFor(p => p.Bestellung, f => f.Random.ListItem(bestellungen))
                .RuleFor(p => p.Artikel, f => f.Random.ListItem(artikel))
                .RuleFor(p => p.Menge, f => f.Random.Int(1, 10))
                .Generate(128);
            Positionen.AddRange(positionen);
            SaveChanges();
        }
    }

}
