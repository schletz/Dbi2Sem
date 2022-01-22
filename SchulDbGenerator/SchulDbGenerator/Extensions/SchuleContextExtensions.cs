using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.Distributions.Gaussian;
using Bogus.Extensions;
using SchulDb.Model;
using System.Text.RegularExpressions;

namespace SchulDbGenerator
{
    public static class SchuleContextExtensions
    {
        public static void SeedDatabase(this SchuleContext db, SchulDb.Untis.Untisdata data, int schuljahr)
        {
            // Wir schreiben nur die Informatikklassen in die Datenbank, sonst ist das Ergebnis der
            // Abfragen unnötig groß.

            var echteKlassen = data.Klassen
                .Where(k => !string.IsNullOrEmpty(k.Nr) && Regex.IsMatch(k.Nr ?? "", @"^[0-9][A-Z_]{2,}$"))
                .ToList();
            if (!echteKlassen.Any())
                throw new SchulDb.SchulDbException("Der Untis Export enthält keine Klassen.");
            // Im Datenmodell darf ein Lehrer zu selben Zeit nur eine Klasse haben. Durch die
            // Gruppierung stellen wir das sicher.
            var echteStunden = data.Stundenplan.Where(s =>
                    !string.IsNullOrEmpty(s.Fach) &&
                    echteKlassen.Any(k => k.Nr == s.Klasse) &&
                    data.Faecher.Any(f => f.Nr == s.Fach))
                .GroupBy(s => new { s.Stunde, s.Tag, s.Lehrer, s.Klasse })
                .Select(g => g.First())
                .ToList();
            if (!echteStunden.Any())
                throw new SchulDb.SchulDbException("Der Untis Export enthält keine Stunden im Stundenplan.");
            // Alle Fächer, die unterrichtet werden. Das schließt Kunstfächer wie Sprechstunde, ... aus.
            var echteFaecher = data.Faecher.Where(f => echteStunden.Any(s => s.Fach == f.Nr)).ToList();
            if (!echteFaecher.Any())
                throw new SchulDb.SchulDbException("Der Untis Export enthält keine Fächer im Stundenplan.");

            Randomizer.Seed = new Random(987);
            DateTime current = new DateTime(schuljahr, 9, 1);
            Faker fkr = new Faker();

            // *********************************************************************************
            // STATISCHE DATEN
            // *********************************************************************************

            var geschlechters = new List<Geschlecht>
            {
                new Geschlecht { GesId = 1, GesMw = "m", GesMaennlichweiblich = "männlich", GesSchuelerschuelerin = "Schüler", GesLehrerlehrerin = "Lehrer" },
                new Geschlecht { GesId = 2, GesMw = "w", GesMaennlichweiblich = "weiblich", GesSchuelerschuelerin = "Schülerin", GesLehrerlehrerin = "Lehrerin" }
            };
            db.Geschlechters.AddRange(geschlechters);
            db.SaveChanges();

            var religionens = new List<Religion>()
            {
                new Religion { RelId = 10, RelNr = "ob", RelName = "Ohne Bekenntnis", RelGesetzlichanerkannt = false, RelStaatlicheingetragen = false },
                new Religion { RelId = 11, RelNr = "evab", RelName = "evangelisch A.B.", RelGesetzlichanerkannt = true, RelStaatlicheingetragen = false },
                new Religion { RelId = 12, RelNr = "islam", RelName = "islamisch", RelGesetzlichanerkannt = true, RelStaatlicheingetragen = false },
                new Religion { RelId = 13, RelNr = "rk", RelName = "römisch - katholisch", RelGesetzlichanerkannt = true, RelStaatlicheingetragen = false }
            };
            db.Religionens.AddRange(religionens);

            db.SaveChanges();

            var schuljahres = new List<Schuljahr>
            {
                new Schuljahr { SjaNr = 20190, SjaBezeichnung = "Schuljahr 2019/20", Wintersemester = true, Sommersemester = true, SjaDatumvon = new DateTime(2019, 9, 2), SjaDatumbis = new DateTime(2020, 7, 3) },
                new Schuljahr { SjaNr = 20191, SjaBezeichnung = "Wintersemester 2019/20", Wintersemester = true, Sommersemester = false, SjaDatumvon = new DateTime(2019, 9, 2), SjaDatumbis = new DateTime(2020, 2, 9) },
                new Schuljahr { SjaNr = 20192, SjaBezeichnung = "Sommersemester 2019/20", Wintersemester = false, Sommersemester = true, SjaDatumvon = new DateTime(2020, 2, 10), SjaDatumbis = new DateTime(2020, 7, 3) },
                new Schuljahr { SjaNr = 20200, SjaBezeichnung = "Schuljahr 2020/21", Wintersemester = true, Sommersemester = true, SjaDatumvon = new DateTime(2020, 9, 7), SjaDatumbis = new DateTime(2021, 7, 2) },
                new Schuljahr { SjaNr = 20201, SjaBezeichnung = "Wintersemester 2020/21", Wintersemester = true, Sommersemester = false, SjaDatumvon = new DateTime(2020, 9, 7), SjaDatumbis = new DateTime(2021, 2, 7) },
                new Schuljahr { SjaNr = 20202, SjaBezeichnung = "Sommersemester 2020/21", Wintersemester = false, Sommersemester = true, SjaDatumvon = new DateTime(2019, 2, 8), SjaDatumbis = new DateTime(2021, 7, 2) },
                new Schuljahr { SjaNr = 20210, SjaBezeichnung = "Schuljahr 2021/22", Wintersemester = true, Sommersemester = true, SjaDatumvon = new DateTime(2021, 9, 6), SjaDatumbis = new DateTime(2022, 7, 1) },
                new Schuljahr { SjaNr = 20211, SjaBezeichnung = "Wintersemester 2021/22", Wintersemester = true, Sommersemester = false, SjaDatumvon = new DateTime(2021, 9, 6), SjaDatumbis = new DateTime(2022, 2, 13) },
                new Schuljahr { SjaNr = 20212, SjaBezeichnung = "Sommersemester 2021/22", Wintersemester = false, Sommersemester = true, SjaDatumvon = new DateTime(2022, 2, 14), SjaDatumbis = new DateTime(2022, 7, 1) }
            };
            db.Schuljahres.AddRange(schuljahres);
            db.SaveChanges();

            var staatens = new List<Staat>
            {
                new Staat { StaNr = "A", StaName = "Österreich", StaStaatsb = "österreichisch", StaEuland = true },
                new Staat { StaNr = "TR", StaName = "Türkei", StaStaatsb = "türkisch", StaEuland = false },
                new Staat { StaNr = "SBM", StaName = "Serbien", StaStaatsb = "serbisch", StaEuland = false },
                new Staat { StaNr = "CRO", StaName = "Kroatien", StaStaatsb = "kroatisch", StaEuland = true },
                new Staat { StaNr = "D", StaName = "Deutschland", StaStaatsb = "Deutschland", StaEuland = true },
                new Staat { StaNr = "SQ", StaName = "Slowakei", StaStaatsb = "slowakisch", StaEuland = true },
                new Staat { StaNr = "SLO", StaName = "Slowenien", StaStaatsb = "slowenisch", StaEuland = true }
            };
            db.Staatens.AddRange(staatens);
            db.SaveChanges();

            var stundenrasters = new List<Stundenraster>
            {
                new Stundenraster { StrNr = 1, StrBeginn = new TimeSpan(8, 00, 0), StrEnde = new TimeSpan(8, 50, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 2, StrBeginn = new TimeSpan(8, 50, 0), StrEnde = new TimeSpan(9, 40, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 3, StrBeginn = new TimeSpan(9, 55, 0), StrEnde = new TimeSpan(10, 45, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 4, StrBeginn = new TimeSpan(10, 45, 0), StrEnde = new TimeSpan(11, 35, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 5, StrBeginn = new TimeSpan(11, 45, 0), StrEnde = new TimeSpan(12, 35, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 6, StrBeginn = new TimeSpan(12, 35, 0), StrEnde = new TimeSpan(13, 25, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 7, StrBeginn = new TimeSpan(13, 25, 0), StrEnde = new TimeSpan(14, 15, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 8, StrBeginn = new TimeSpan(14, 25, 0), StrEnde = new TimeSpan(15, 15, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 9, StrBeginn = new TimeSpan(15, 15, 0), StrEnde = new TimeSpan(16, 05, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 10, StrBeginn = new TimeSpan(16, 15, 0), StrEnde = new TimeSpan(17, 05, 0), StrIstAbend = false },
                new Stundenraster { StrNr = 11, StrBeginn = new TimeSpan(17, 10, 0), StrEnde = new TimeSpan(17, 55, 0), StrIstAbend = true },
                new Stundenraster { StrNr = 12, StrBeginn = new TimeSpan(17, 55, 0), StrEnde = new TimeSpan(18, 40, 0), StrIstAbend = true },
                new Stundenraster { StrNr = 13, StrBeginn = new TimeSpan(18, 50, 0), StrEnde = new TimeSpan(19, 35, 0), StrIstAbend = true },
                new Stundenraster { StrNr = 14, StrBeginn = new TimeSpan(19, 35, 0), StrEnde = new TimeSpan(20, 20, 0), StrIstAbend = true },
                new Stundenraster { StrNr = 15, StrBeginn = new TimeSpan(20, 30, 0), StrEnde = new TimeSpan(21, 15, 0), StrIstAbend = true },
                new Stundenraster { StrNr = 16, StrBeginn = new TimeSpan(21, 15, 0), StrEnde = new TimeSpan(22, 00, 0), StrIstAbend = true }
            };
            db.Stundenrasters.AddRange(stundenrasters);
            db.SaveChanges();

            // *********************************************************************************
            // DATEN AUS UNTIS
            // *********************************************************************************
            // Die Räume der Schule lesen und eintragen.
            var raeumes = data.Raeume
                .Where(r => Regex.IsMatch(r.Nr, @"^[ABCD].\..+"))
                .Select(r => new Raum
                {
                    RId = r.Nr,
                    RPlaetze = r.Kapaz,
                    RArt = r.Art == "Stammklasse" ? "Klassenraum" : r.Art
                })
                .ToList();
            db.Raeumes.AddRange(raeumes);
            db.SaveChanges();

            // Die "echten" Lehrer der Schule (die eine Sprechstunde eingetragen haben) lesen und eintragen.
            var sprechstunden = data.Stundenplan.Where(s => s.Fach == "S").GroupBy(s => s.Lehrer).ToDictionary(s => s.Key, s => s.First());
            if (!sprechstunden.Any())
            {
                throw new SchulDb.SchulDbException("Es wurden keine Sprechstunden (Fach = S) im Stundenplanfile gefunden.");
            }
            var lehrers = data.Lehrer
                .Where(l =>
                    !string.IsNullOrEmpty(l.Nr) && !string.IsNullOrEmpty(l.Vorname) && !string.IsNullOrEmpty(l.Zuname))
                .Select(l =>
                {
                    var gebdat = fkr.Date.Between(current.AddYears(-65), current.AddYears(-25)).Date;
                    var eintrittsjahr = fkr.Random.Int(gebdat.Year + 24, current.Year);
                    return new Lehrer
                    {
                        LNr = l.Nr,
                        LName = l.Zuname,
                        LVorname = l.Vorname,
                        LGebdat = gebdat.OrNull(fkr, 0.2f),
                        LGehalt = Math.Round(2000M +
                                       (current.Year - eintrittsjahr) * 100M +
                                       fkr.Random.GaussianDecimal(0, 100)).OrNull(fkr, 0.2f),
                        LEintrittsjahr = eintrittsjahr.OrNull(fkr, 0.2f),
                        LSprechstunde = sprechstunden.TryGetValue(l.Nr, out var spr) ? SchulDb.Untis.Untisdata.Wochentage[spr.Tag - 1] + spr.Stunde.ToString() : null
                    };
                })
                .ToList();
            db.Lehrers.AddRange(lehrers);
            db.SaveChanges();

            // Die Abteilungen mit den oben eingetragenen Lehrern verknüpfen. Die ID ist die
            // 3. - 5. Stelle (3AFITM --> FIT)
            var abteilungens = new List<Abteilung>
            {
                new Abteilung { AbtNr = "O", AbtName = "Übergangsstufe", AbtLeiterNavigation = lehrers.First(l => l.LNr == "ZLA") },
                new Abteilung { AbtNr = "FIT", AbtName = "Fachschule für Informationstechnik", AbtLeiterNavigation = lehrers.First(l => l.LNr == "HEB") },
                new Abteilung { AbtNr = "HBG", AbtName = "Höhere Lehranstalt für Biomedizin- und Gesundheitstechnik", AbtLeiterNavigation = lehrers.First(l => l.LNr == "HEB") },
                new Abteilung { AbtNr = "HIF", AbtName = "Höhere Lehranstalt für Informatik", AbtLeiterNavigation = lehrers.First(l => l.LNr == "JEL") },
                new Abteilung { AbtNr = "HMN", AbtName = "Höhere Lehranstalt für Medien", AbtLeiterNavigation = lehrers.First(l => l.LNr == "PRW") },
                new Abteilung { AbtNr = "HKU", AbtName = "Höhere Lehranstalt für Kunst", AbtLeiterNavigation = lehrers.First(l => l.LNr == "PRW") },
                new Abteilung { AbtNr = "HWI", AbtName = "Höhere Lehranstalt für Wirtschaftsingenieure", AbtLeiterNavigation = lehrers.First(l => l.LNr == "ZLA") },
                new Abteilung { AbtNr = "AIF", AbtName = "Aufbaulehrgang Tagesform", AbtLeiterNavigation = lehrers.First(l => l.LNr == "STH") },
                new Abteilung { AbtNr = "BIF", AbtName = "Aufbaulehrgang Abendform", AbtLeiterNavigation = lehrers.First(l => l.LNr == "STH") },
                new Abteilung { AbtNr = "CIF", AbtName = "Kolleg Abendform", AbtLeiterNavigation = lehrers.First(l => l.LNr == "STH") },
                new Abteilung { AbtNr = "KIF", AbtName = "Kolleg Tagesform", AbtLeiterNavigation = lehrers.First(l => l.LNr == "STH") },
                new Abteilung { AbtNr = "VIF", AbtName = "Vorbereitungslehrgang", AbtLeiterNavigation = lehrers.First(l => l.LNr == "STH") },
                new Abteilung { AbtNr = "KKU", AbtName = "Kolleg für Design", AbtLeiterNavigation = lehrers.First(l => l.LNr == "PRW") },
                new Abteilung { AbtNr = "CMN", AbtName = "Kolleg für Medien", AbtLeiterNavigation = lehrers.First(l => l.LNr == "PRW") },
                new Abteilung { AbtNr = "BKU", AbtName = "Kolleg für Audivisuelles Mediendesign", AbtLeiterNavigation = lehrers.First(l => l.LNr == "PRW") }
            };
            db.Abteilungens.AddRange(abteilungens);
            db.SaveChanges();

            foreach (var l in lehrers)
            {
                if (l.LNr == "HG") { l.LChefNavigation = null; }
                else if (abteilungens.Any(a => a.AbtLeiterNavigation.LNr == l.LNr))
                    l.LChefNavigation = lehrers.First(l => l.LNr == "HG");
                else
                    l.LChefNavigation = fkr.Random.ListItem(abteilungens).AbtLeiterNavigation;
            }
            db.SaveChanges();

            // Gegenstände, die unterrichtet werden, eintragen.
            var gegenstaendes = echteFaecher.Select(f => new Gegenstand
            {
                GNr = f.Nr,
                GBez = f.Langname
            })
            .ToList();
            db.Gegenstaendes.AddRange(gegenstaendes);
            db.SaveChanges();

            // Klassen eintragen.
            var klassens = echteKlassen.Select(k =>
            {
                var sjahr = schuljahres.First(s => s.SjaNr == current.Year * 10 + k.Jahresform);
                return new Klasse
                {
                    KNr = k.Nr,
                    KSchuljahrNavigation = sjahr,
                    KBez = k.Beschreibung,
                    // Bei Jahresformen wird die Schulstufe (9, 10, ...) eingetragen.
                    KSchulstufe = k.Jahresform == 0 ? k.Schulstufe : null,
                    KStammraumNavigation = raeumes.FirstOrDefault(r => r.RId == k.Stammraum),
                    KVorstandNavigation = lehrers.FirstOrDefault(l => l.LNr == k.Kv) ?? throw new SchulDb.SchulDbException($"KV zur Klasse {k.Nr} nicht vorhanden!"),
                    KAbteilungNavigation = abteilungens.FirstOrDefault(a => a.AbtNr == k.Nr.Substring(2, Math.Min(3, k.Nr.Length - 2)))
                        ?? throw new SchulDb.SchulDbException($"Abteilung zur Klasse {k.Nr} nicht vorhanden!"),
                    KDatumbis = k.Abschlussklasse ? new DateTime(current.Year + 1, 5, 1) : null
                };
            })
            .ToList();
            db.Klassens.AddRange(klassens);
            db.SaveChanges();

            // Den Stundenplan der eintragen.
            var stundens = echteStunden.Select(s => new Stunde
            {
                StStunde = s.Stunde,
                StTag = s.Tag,
                StLehrerNavigation = lehrers.FirstOrDefault(l => l.LNr == s.Lehrer) ?? throw new SchulDb.SchulDbException($"Lehrer zur Stunde {s.UntId} nicht vorhanden!"),
                StKlasseNavigation = klassens.FirstOrDefault(k => k.KNr == s.Klasse) ?? throw new SchulDb.SchulDbException($"Klasse zur Stunde {s.UntId} nicht vorhanden!"),
                StGegenstandNavigation = gegenstaendes.FirstOrDefault(g => g.GNr == s.Fach) ?? throw new SchulDb.SchulDbException($"Gegenstand zur Stunde {s.UntId} nicht vorhanden!"),
                StRaumNavigation = raeumes.FirstOrDefault(r => r.RId == s.Raum)
            })
            .ToList();
            db.Stundens.AddRange(stundens);
            db.SaveChanges();

            // *********************************************************************************
            // ERGÄNZUNG MIT ZUFALLSDATEN (Schüler, Prüfungen, ...)
            // *********************************************************************************
            // Die Schüler in den Klassen generieren, die im Wintersemester vorhanden sind
            int sid = 1000;
            var orte = (data.Adressen.Orte.Where(o => o.Bundesland == "W").Take(23)
                .Concat(data.Adressen.Orte.Where(o => o.Bundesland == "N").Take(10))
                .Concat(data.Adressen.Orte.Where(o => o.Bundesland == "B").Take(10))).ToList();
            var staaten = staatens.OrderBy(s => s.StaNr).ToArray();
            var religionen = religionens.OrderBy(r => r.RelId).ToArray();
            var schuelerFaker = new Faker<Schueler>()
                .Rules((f, s) =>
                {
                    var ort = f.Random.ListItem(orte);
                    s.SNr = sid++;
                    s.SZuname = f.Name.LastName();
                    s.SStrasse = f.Random.ListItem(data.Adressen.Strassen).Name.OrDefault(f, 0.2f);
                    s.SHausnummer =
                        (f.Random.Int(1, 100).ToString() +
                            (f.Random.Bool(0.2f) ? "/" + f.Random.Int(1, 100).ToString() : ""))
                        .OrDefault(f, 0.2f);
                    s.SPostleitzahl = ort.Plz.OrNull(f, 0.2f);
                    s.SOrt = ort.Name.OrDefault(f, 0.2f);
                    s.SStaatsbNavigation = f.Random.WeightedRandom(
                        staaten,
                        new float[] { 0.5f, 0.25f, 0f, 0.03125f, 0.03125f, 0.03125f, 0.125f }).OrDefault(f, 0.2f);
                    s.SReligionNavigation = f.Random.WeightedRandom(
                        religionen,
                        new float[] { 0.3f, 0f, 0.2f, 0.4f }).OrDefault(f, 0.2f);
                });

            var schuelers = new List<Schueler>();
            foreach (var k in klassens.Where(k => k.KSchuljahrNavigation.Wintersemester))
            {
                int sAnz = k.KSchulstufe == 9 ? 32 : fkr.Random.GaussianInt(26, 2);
                float percentFemale = 0.5f;
                try
                {
                    if (k.KNr.Substring(1, 4) == "AHIF") { percentFemale = 0.2f; } else if (k.KNr.Substring(1, 4) == "EHIF") { percentFemale = 0.2f; } else if (k.KNr.Substring(2, 3) == "HIF") { percentFemale = 0f; }
                }
                catch { }

                var schueler = schuelerFaker
                    .Rules((f, s) =>
                    {
                        var geschl = f.Random.Float() < percentFemale ?
                            Bogus.DataSets.Name.Gender.Female :
                            Bogus.DataSets.Name.Gender.Male;
                        s.SKlasseNavigation = k;
                        s.SVorname = f.Name.FirstName(geschl);
                        s.SGebdatum = k.KSchulstufe != null ?
                            f.Date.Between(
                                current.AddYears(-(int)k.KSchulstufe - 6),
                                current.AddYears(-(int)k.KSchulstufe - 5)).Date.OrNull(f, 0.2f) :
                            f.Date.Between(
                                current.AddYears(-30),
                                current.AddYears(-20)).Date.OrNull(f, 0.2f);
                        s.SGeschlechtNavigation = geschl == Bogus.DataSets.Name.Gender.Male ?
                            geschlechters.First(g => g.GesId == 1) :
                            geschlechters.First(g => g.GesId == 2);
                    })
                    .Generate(sAnz);

                schuelers.AddRange(schueler);
            }
            db.Schuelers.AddRange(schuelers);
            db.SaveChanges();

            // Die Klassensprecher und Stellverträter aus der Liste der Schüler der Klasse wählen
            foreach (var k in klassens)
            {
                var kandidaten = fkr.Random.Shuffle(schuelers.Where(s => s.SKlasseNavigation == k)).Take(2).ToArray();
                if (kandidaten.Length < 2) { continue; }
                k.KKlasprNavigation = kandidaten[0].OrDefault(fkr, 0.2f);
                if (k.KKlasprNavigation != null)
                {
                    k.KKlasprstvNavigation = kandidaten[1].OrDefault(fkr, 0.2f);
                }
            }
            db.SaveChanges();

            // Prüfungen für Klassen mit Schülern generieren
            var pruefarten = new Dictionary<string, string>
            {
                { "D", "s" }, { "DUK", "s" }, { "Dx", "s" }, { "Dy", "s" },
                { "AM", "s" }, { "AMx", "s" }, { "AMy", "s" },
                { "E1", "s" }, { "E1x", "s" }, { "E1y", "s" },
                { "POS1", "p" }, { "POS1x", "p" }, { "POS1y", "p" }, { "POS1z", "p" },
                { "DBI1", "p" }, { "DBI1x", "p" }, { "DBI1y", "p" }, { "DBI2x", "p" }, { "DBI2y", "p" }
            };
            var pruefungen = new List<Pruefung>();
            foreach (var klasse in klassens)
            {
                var schueler = schuelers.Where(s => s.SKlasseNavigation == klasse).ToList();
                if (!schueler.Any()) { continue; }
                var pruefGegenstaende = stundens.Where(s => s.StKlasseNavigation == klasse).GroupBy(s => s.StGegenstand).Select(g => g.First()).ToList();
                var stunden = fkr.Random.ListItems(pruefGegenstaende, Math.Min(5, pruefGegenstaende.Count));
                foreach (var stunde in stunden)
                {
                    var pruefart = pruefarten.ContainsKey(stunde.StGegenstand) ?
                        pruefarten[stunde.StGegenstand] :
                        fkr.Random.WeightedRandom(
                            new string[] { "s", "m", "p" },
                            new float[] { 0.4f, 0.4f, 0.2f }).OrDefault(fkr, 0.2f);
                    var besteNote = fkr.Random.Int(1, 2);
                    var schlechtesteNote = fkr.Random.Int(4, 5);
                    var pruefFaker = new Faker<Pruefung>()
                        .Rules((f, p) =>
                        {
                            p.PArt = pruefart;
                            // Ganze 5 Minuten zwischen 8 und 22 Uhr generieren.
                            p.PDatumZeit =
                                f.Date.Between(current, current.AddMonths(9)).Date +
                                new TimeSpan(f.Random.Int(8, 21), f.Random.Int(0, 11) * 5, 0);
                            p.PNote = f.Random
                                .Int(besteNote, schlechtesteNote)
                                .OrNull(f, 0.2f);
                            p.PGegenstandNavigation = stunde.StGegenstandNavigation;
                            // In 10% der Fälle nehmen wir irgeneinen Prüfer, der das Fach unterrichtet,
                            // aber nicht notwendigerweise in der Klasse steht.
                            p.PPrueferNavigation = f.Random.Bool(0.1f) ?
                                p.PPrueferNavigation = f.Random.ListItem(db.Stundens
                                    .Where(s => s.StGegenstand == stunde.StGegenstand)
                                    .Select(s => s.StLehrerNavigation)
                                    .ToList()) :
                                p.PPrueferNavigation = stunde.StLehrerNavigation;
                            p.PKandidatNavigation = f.Random.ListItem(schueler);
                        });
                    pruefungen.AddRange(pruefFaker.Generate(fkr.Random.Int(0, 6)));
                }
            }
            // Prüfungen könnten doppelt sein, daher nehmen wir nur den ersten Datensatz.
            db.Pruefungens.AddRange(pruefungen
                .GroupBy(p => new { p.PDatumZeit, p.PPruefer, p.PGegenstand })
                .Select(g => g.FirstOrDefault()));
            db.SaveChanges();
        }
    }
}