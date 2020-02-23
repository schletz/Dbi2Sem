using System;
using Bogus;
using Bogus.Extensions;
using Bogus.Distributions.Gaussian;
using System.Collections.Generic;
using System.Linq;

namespace Tankstelle
{
    class Program
    {
        static void Main(string[] args)
        {
            Randomizer.Seed = new Random(104950);
            var fkr = new Faker();
            using (TankstelleContext db = new TankstelleContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                DateTime tag = new DateTime(2000, 1, 1);
                DateTime endJahrhundert = new DateTime(2100, 1, 1);
                int i = 0;
                while (tag < endJahrhundert)
                {
                    db.Tage.Add(new Tag
                    {
                        Id = i++,
                        Datum = tag,
                        Monat = tag.Month,
                        Jahr = tag.Year,
                        Wochentag = ((int)tag.DayOfWeek == 0) ? 7 : (int)tag.DayOfWeek
                    });
                    tag += TimeSpan.FromDays(1);
                }
                db.SaveChanges();

                var kategorien = new Kategorie[]
                {
                    new Kategorie{Name = "Diesel"},
                    new Kategorie{Name = "Benzin normal"},
                    new Kategorie{Name = "Benzin super"},
                };


                db.Kategorien.AddRange(kategorien);
                db.SaveChanges();

                var tankstelleFaker = new Faker<Tankstelle>()
                    .RuleFor(t => t.Adresse, f => f.Address.StreetAddress())
                    .RuleFor(t => t.Ort, f => f.Address.City())
                    .RuleFor(t => t.Plz, f => f.Random.Int(1000, 9999));
                var tankstellen = tankstelleFaker.Generate(2).ToArray();
                db.Tankstellen.AddRange(tankstellen);
                db.SaveChanges();

                var preisFaker = new Faker<Preis>()
                    .RuleFor(p => p.Tankstelle, f => f.Random.ListItem(tankstellen))
                    .RuleFor(p => p.Kategorie, f => f.Random.ListItem(kategorien));

                DateTime start = new DateTime(2019, 1, 1);
                DateTime end = new DateTime(2020, 1, 1);
                while (start < end)
                {
                    var preis = preisFaker
                        .RuleFor(p => p.GueltigVon, f => start)
                        .Generate();
                    var alterPreis = db.Preise.SingleOrDefault(
                        p => p.Tankstelle == preis.Tankstelle &&
                        p.Kategorie == preis.Kategorie &&
                        p.GueltigBis == null);
                    if (alterPreis != null)
                    {
                        alterPreis.GueltigBis = start;
                        preis.Wert = alterPreis.Wert + Math.Round(fkr.Random.GaussianDecimal(0, 0.01), 4);
                    }
                    else
                    {
                        preis.Wert = preis.Kategorie.Name switch
                        {
                            "Diesel" => 1.1M,
                            "Benzin normal" => 1.2M,
                            "Benzin super" => 1.3M,
                            _ => 1M
                        };
                    }
                    var verkaeufe = Enumerable.Range(0, fkr.Random.Int(0, 4))
                    .Select(i => new Verkauf
                    {
                        Datum = fkr.Date.Between(start, start + TimeSpan.FromDays(10)),
                        Menge = Math.Round(fkr.Random.GaussianDecimal(50, 10), 2),
                        Preis = preis
                    });

                    db.Preise.Add(preis);
                    db.Verkaeufe.AddRange(verkaeufe);
                    db.SaveChanges();
                    start += TimeSpan.FromDays(10);
                }
            }
        }
    }
}
