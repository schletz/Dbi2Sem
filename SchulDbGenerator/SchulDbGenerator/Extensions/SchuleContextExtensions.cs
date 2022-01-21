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
            var echteKlassen = (from k in data.Klassen
                                where Regex.IsMatch(k.Nr ?? "", @"^[0-9][A-Z_]{2,}$", RegexOptions.IgnoreCase)
                                select k).ToList();
            if (!echteKlassen.Any())
                throw new SchulDb.SchulDbException("Der Untis Export enthält keine Klassen.");
            // Im Datenmodell darf ein Lehrer zu selben Zeit nur eine Klasse haben. Durch die
            // Gruppierung stellen wir das sicher.
            var echteStunden = (from s in data.Stundenplan
                                join k in echteKlassen on s.Klasse equals k.Nr
                                where Regex.IsMatch(s.Fach ?? "", @"^[^FR]", RegexOptions.IgnoreCase)
                                group s by new { s.Stunde, s.Tag, s.Lehrer, s.Klasse } into g
                                select g.FirstOrDefault()).ToList();
            if (!echteStunden.Any())
                throw new SchulDb.SchulDbException("Der Untis Export enthält keine Stunden im Stundenplan.");
            // Alle Fächer, die unterrichtet werden. Das schließt Kunstfächer wie Sprechstunde, ... aus.
            var echteFaecher = (from f in data.Faecher
                                join s in echteStunden on f.Nr equals s.Fach into sg
                                where sg.Any()
                                select f).ToList();
            if (!echteFaecher.Any())
                throw new SchulDb.SchulDbException("Der Untis Export enthält keine Fächer im Stundenplan.");

            Randomizer.Seed = new Random(987);
            DateTime current = new DateTime(schuljahr, 9, 1);
            Faker fkr = new Faker();

            // *********************************************************************************
            // STATISCHE DATEN
            // *********************************************************************************

            db.Geschlechters.Add(new Geschlecht { GesId = 1, GesMw = "m", GesMaennlichweiblich = "männlich", GesSchuelerschuelerin = "Schüler", GesLehrerlehrerin = "Lehrer" });
            db.Geschlechters.Add(new Geschlecht { GesId = 2, GesMw = "w", GesMaennlichweiblich = "weiblich", GesSchuelerschuelerin = "Schülerin", GesLehrerlehrerin = "Lehrerin" });
            db.SaveChanges();

            db.Religionens.Add(new Religion { RelId = 10, RelNr = "ob", RelName = "Ohne Bekenntnis", RelGesetzlichanerkannt = false, RelStaatlicheingetragen = false });
            db.Religionens.Add(new Religion { RelId = 11, RelNr = "evab", RelName = "evangelisch A.B.", RelGesetzlichanerkannt = true, RelStaatlicheingetragen = false });
            db.Religionens.Add(new Religion { RelId = 12, RelNr = "islam", RelName = "islamisch", RelGesetzlichanerkannt = true, RelStaatlicheingetragen = false });
            db.Religionens.Add(new Religion { RelId = 13, RelNr = "rk", RelName = "römisch - katholisch", RelGesetzlichanerkannt = true, RelStaatlicheingetragen = false });
            db.SaveChanges();

            db.Schuljahres.Add(new Schuljahr { SjaNr = 20180, SjaBezeichnung = "Schuljahr 2018/19", Wintersemester = true, Sommersemester = true, SjaDatumvon = new DateTime(2018, 9, 3), SjaDatumbis = new DateTime(2019, 6, 28) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20181, SjaBezeichnung = "Wintersemester 2018/19", Wintersemester = true, Sommersemester = false, SjaDatumvon = new DateTime(2018, 9, 3), SjaDatumbis = new DateTime(2019, 2, 10) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20182, SjaBezeichnung = "Sommersemester 2018/19", Wintersemester = false, Sommersemester = true, SjaDatumvon = new DateTime(2019, 2, 11), SjaDatumbis = new DateTime(2019, 6, 28) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20190, SjaBezeichnung = "Schuljahr 2019/20", Wintersemester = true, Sommersemester = true, SjaDatumvon = new DateTime(2019, 9, 2), SjaDatumbis = new DateTime(2020, 7, 3) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20191, SjaBezeichnung = "Wintersemester 2019/20", Wintersemester = true, Sommersemester = false, SjaDatumvon = new DateTime(2019, 9, 2), SjaDatumbis = new DateTime(2020, 2, 9) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20192, SjaBezeichnung = "Sommersemester 2019/20", Wintersemester = false, Sommersemester = true, SjaDatumvon = new DateTime(2020, 2, 10), SjaDatumbis = new DateTime(2020, 7, 3) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20200, SjaBezeichnung = "Schuljahr 2020/21", Wintersemester = true, Sommersemester = true, SjaDatumvon = new DateTime(2020, 9, 7), SjaDatumbis = new DateTime(2021, 7, 2) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20201, SjaBezeichnung = "Wintersemester 2020/21", Wintersemester = true, Sommersemester = false, SjaDatumvon = new DateTime(2020, 9, 7), SjaDatumbis = new DateTime(2021, 2, 7) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20202, SjaBezeichnung = "Sommersemester 2020/21", Wintersemester = false, Sommersemester = true, SjaDatumvon = new DateTime(2019, 2, 8), SjaDatumbis = new DateTime(2021, 7, 2) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20210, SjaBezeichnung = "Schuljahr 2021/22", Wintersemester = true, Sommersemester = true, SjaDatumvon = new DateTime(2021, 9, 6), SjaDatumbis = new DateTime(2022, 7, 1) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20211, SjaBezeichnung = "Wintersemester 2021/22", Wintersemester = true, Sommersemester = false, SjaDatumvon = new DateTime(2021, 9, 6), SjaDatumbis = new DateTime(2022, 2, 13) });
            db.Schuljahres.Add(new Schuljahr { SjaNr = 20212, SjaBezeichnung = "Sommersemester 2021/22", Wintersemester = false, Sommersemester = true, SjaDatumvon = new DateTime(2022, 2, 14), SjaDatumbis = new DateTime(2022, 7, 1) });
            db.SaveChanges();

            db.Staatens.Add(new Staat { StaNr = "A", StaName = "Österreich", StaStaatsb = "österreichisch", StaEuland = true });
            db.Staatens.Add(new Staat { StaNr = "TR", StaName = "Türkei", StaStaatsb = "türkisch", StaEuland = false });
            db.Staatens.Add(new Staat { StaNr = "SBM", StaName = "Serbien", StaStaatsb = "serbisch", StaEuland = false });
            db.Staatens.Add(new Staat { StaNr = "CRO", StaName = "Kroatien", StaStaatsb = "kroatisch", StaEuland = false });
            db.Staatens.Add(new Staat { StaNr = "D", StaName = "Deutschland", StaStaatsb = "Deutschland", StaEuland = true });
            db.Staatens.Add(new Staat { StaNr = "SQ", StaName = "Slowakei", StaStaatsb = "slowakisch", StaEuland = true });
            db.Staatens.Add(new Staat { StaNr = "SLO", StaName = "Slowenien", StaStaatsb = "slowenisch", StaEuland = true });
            db.SaveChanges();

            db.Stundenrasters.Add(new Stundenraster { StrNr = 1, StrBeginn = new TimeSpan(8, 00, 0), StrEnde = new TimeSpan(8, 50, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 2, StrBeginn = new TimeSpan(8, 50, 0), StrEnde = new TimeSpan(9, 40, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 3, StrBeginn = new TimeSpan(9, 55, 0), StrEnde = new TimeSpan(10, 45, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 4, StrBeginn = new TimeSpan(10, 45, 0), StrEnde = new TimeSpan(11, 35, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 5, StrBeginn = new TimeSpan(11, 45, 0), StrEnde = new TimeSpan(12, 35, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 6, StrBeginn = new TimeSpan(12, 35, 0), StrEnde = new TimeSpan(13, 25, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 7, StrBeginn = new TimeSpan(13, 25, 0), StrEnde = new TimeSpan(14, 15, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 8, StrBeginn = new TimeSpan(14, 25, 0), StrEnde = new TimeSpan(15, 15, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 9, StrBeginn = new TimeSpan(15, 15, 0), StrEnde = new TimeSpan(16, 05, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 10, StrBeginn = new TimeSpan(16, 15, 0), StrEnde = new TimeSpan(17, 05, 0), StrIstAbend = false });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 11, StrBeginn = new TimeSpan(17, 10, 0), StrEnde = new TimeSpan(17, 55, 0), StrIstAbend = true });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 12, StrBeginn = new TimeSpan(17, 55, 0), StrEnde = new TimeSpan(18, 40, 0), StrIstAbend = true });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 13, StrBeginn = new TimeSpan(18, 50, 0), StrEnde = new TimeSpan(19, 35, 0), StrIstAbend = true });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 14, StrBeginn = new TimeSpan(19, 35, 0), StrEnde = new TimeSpan(20, 20, 0), StrIstAbend = true });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 15, StrBeginn = new TimeSpan(20, 30, 0), StrEnde = new TimeSpan(21, 15, 0), StrIstAbend = true });
            db.Stundenrasters.Add(new Stundenraster { StrNr = 16, StrBeginn = new TimeSpan(21, 15, 0), StrEnde = new TimeSpan(22, 00, 0), StrIstAbend = true });
            db.SaveChanges();

            // *********************************************************************************
            // DATEN AUS UNTIS
            // *********************************************************************************

            // Die Räume der Schule lesen und eintragen.
            db.Raeumes.AddRange(from r in data.Raeume
                                where
                                    r.Nr[0] == 'A' || r.Nr[0] == 'B' ||
                                    r.Nr[0] == 'C' || r.Nr[0] == 'D'
                                select new Raum
                                {
                                    RId = r.Nr,
                                    RPlaetze = r.Kapaz,
                                    // Stammklasse sorgt für Verwirrung, da auch der Stammraum
                                    // der Klasse gemeint sein kann.
                                    RArt = r.Art == "Stammklasse" ? "Klassenraum" : r.Art
                                });
            db.SaveChanges();

            // Die Lehrer der Schule lesen und eintragen.
            db.Lehrers.AddRange(from l in data.Lehrer
                                join s in data.Stundenplan.Where(s => s.Klasse == "SPR") on l.Nr equals s.Lehrer into sg
                                let gebdat = fkr.Date.Between(
                                    current.AddYears(-65),
                                    current.AddYears(-25)).Date
                                let eintrittsjahr = fkr.Random.Int(
                                    gebdat.Year + 24,
                                    current.Year)
                                let spr = sg.FirstOrDefault()
                                select new Lehrer
                                {
                                    LNr = l.Nr,
                                    LName = l.Zuname,
                                    LVorname = l.Vorname,
                                    LGebdat = gebdat.OrNull(fkr, 0.2f),
                                    LGehalt = Math.Round(2000M +
                                            (current.Year - eintrittsjahr) * 100M +
                                            fkr.Random.GaussianDecimal(0, 100)).OrNull(fkr, 0.2f),
                                    LEintrittsjahr = eintrittsjahr.OrNull(fkr, 0.2f),
                                    LSprechstunde = spr != null ? SchulDb.Untis.Untisdata.Wochentage[spr.Tag - 1] +
                                        spr.Stunde.ToString() : null
                                });
            db.SaveChanges();

            // Die Abteilungen mit den oben eingetragenen Lehrern verknüpfen. Die ID ist die
            // 3. - 5. Stelle (3AFITM --> FIT)
            db.Abteilungens.Add(new Abteilung { AbtNr = "O", AbtName = "Übergangsstufe" });
            db.Abteilungens.Add(new Abteilung { AbtNr = "FIT", AbtName = "Fachschule für Informationstechnik", AbtLeiterNavigation = db.Lehrers.Find("HEB") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "HBG", AbtName = "Höhere Lehranstalt für Biomedizin- und Gesundheitstechnik", AbtLeiterNavigation = db.Lehrers.Find("HEB") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "HIF", AbtName = "Höhere Lehranstalt für Informatik", AbtLeiterNavigation = db.Lehrers.Find("JEL") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "HMN", AbtName = "Höhere Lehranstalt für Medien", AbtLeiterNavigation = db.Lehrers.Find("PRW") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "HKU", AbtName = "Höhere Lehranstalt für Kunst", AbtLeiterNavigation = db.Lehrers.Find("PRW") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "HWI", AbtName = "Höhere Lehranstalt für Wirtschaftsingenieure", AbtLeiterNavigation = db.Lehrers.Find("ZLA") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "AIF", AbtName = "Aufbaulehrgang Tagesform", AbtLeiterNavigation = db.Lehrers.Find("STH") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "BIF", AbtName = "Aufbaulehrgang Abendform", AbtLeiterNavigation = db.Lehrers.Find("STH") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "CIF", AbtName = "Kolleg Abendform", AbtLeiterNavigation = db.Lehrers.Find("STH") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "KIF", AbtName = "Kolleg Tagesform", AbtLeiterNavigation = db.Lehrers.Find("STH") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "VIF", AbtName = "Vorbereitungslehrgang", AbtLeiterNavigation = db.Lehrers.Find("STH") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "KKU", AbtName = "Kolleg für Design", AbtLeiterNavigation = db.Lehrers.Find("PRW") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "CMN", AbtName = "Kolleg für Medien", AbtLeiterNavigation = db.Lehrers.Find("PRW") });
            db.Abteilungens.Add(new Abteilung { AbtNr = "BKU", AbtName = "Kolleg für Audivisuelles Mediendesign", AbtLeiterNavigation = db.Lehrers.Find("PRW") });
            db.SaveChanges();

            // Gegenstände, die in den Informatikklassen unterrichtet werden, eintragen.
            db.Gegenstaendes.AddRange(from f in echteFaecher
                                      select new Gegenstand
                                      {
                                          GNr = f.Nr,
                                          GBez = f.Langname
                                      });
            db.SaveChanges();

            // Klassen aus den Informatikabteilungen eintragen.
            db.Klassens.AddRange(from k in echteKlassen
                                 let sjahr = db.Schuljahres.Find(current.Year * 10 + k.Jahresform)
                                 let abt = k.Nr.Substring(2, Math.Min(3, k.Nr.Length - 2))
                                 select new Klasse
                                 {
                                     KNr = k.Nr,
                                     KSchuljahrNavigation = sjahr,
                                     KBez = k.Beschreibung,
                                     // Bei Jahresformen wird die Schulstufe (9, 10, ...) eingetragen.
                                     KSchulstufe = k.Jahresform == 0 ? k.Schulstufe : null,
                                     KStammraumNavigation = db.Raeumes.Find(k.Stammraum),
                                     KVorstandNavigation = db.Lehrers.Find(k.Kv),
                                     KAbteilungNavigation = db.Abteilungens.Find(abt) ?? throw new SchulDb.SchulDbException($"Abteilung {abt} nicht vorhanden!"),
                                     KDatumbis = k.Abschlussklasse ? new DateTime(current.Year + 1, 5, 1) : (DateTime?)null
                                 });
            db.SaveChanges();

            // Den Stundenplan der Informatikklassen eintragen.
            db.Stundens.AddRange(from s in echteStunden
                                 select new Stunde
                                 {
                                     StStunde = s.Stunde,
                                     StTag = s.Tag,
                                     StLehrerNavigation = db.Lehrers.Find(s.Lehrer),
                                     StKlasseNavigation = db.Klassens.Find(s.Klasse),
                                     StGegenstandNavigation = db.Gegenstaendes.Find(s.Fach),
                                     StRaumNavigation = db.Raeumes.Find(s.Raum)
                                 });
            db.SaveChanges();

            // *********************************************************************************
            // ERGÄNZUNG MIT ZUFALLSDATEN (Schüler, Prüfungen, ...)
            // *********************************************************************************

            // Den AV zufällig zuordnen. Die AVs haben HG als Chef, HG hat null.
            var abtLeiter = db.Abteilungens.Select(a => a.AbtLeiterNavigation.LNr).ToHashSet();
            foreach (var l in db.Lehrers)
            {
                if (l.LNr == "HG") { l.LChefNavigation = null; }
                else if (abtLeiter.Contains(l.LNr))
                    l.LChefNavigation = db.Lehrers.Find("HG");
                else
                    l.LChefNavigation = fkr.Random.ListItem(
                        db.Abteilungens
                        .Select(a => a.AbtLeiterNavigation).ToList());
            }
            db.SaveChanges();

            // Die Schüler in den Klassen generieren, die im Wintersemester vorhanden sind
            int sid = 1000;
            var orte = (data.Adressen.Orte.Where(o => o.Bundesland == "W").Take(23)
                .Concat(data.Adressen.Orte.Where(o => o.Bundesland == "N").Take(10))
                .Concat(data.Adressen.Orte.Where(o => o.Bundesland == "B").Take(10))).ToList();
            var staaten = db.Staatens.OrderBy(s => s.StaNr).ToArray();
            var religionen = db.Religionens.OrderBy(r => r.RelId).ToArray();
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

            foreach (var k in db.Schuljahres.Where(s => s.Wintersemester).SelectMany(s => s.Klassens))
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
                            db.Geschlechters.Find(1) :
                            db.Geschlechters.Find(2);
                    })
                    .Generate(sAnz);

                db.Schuelers.AddRange(schueler);
            }
            db.SaveChanges();

            // Die Klassensprecher und Stellverträter aus der Liste der Schüler der Klasse wählen
            foreach (var k in db.Klassens.Where(k => k.Schuelers.Any()))
            {
                k.KKlasprNavigation = fkr.Random.ListItem(k.Schuelers.ToList()).OrDefault(fkr, 0.2f);
                if (k.KKlasprNavigation != null)
                {
                    k.KKlasprstvNavigation = fkr.Random.ListItem(
                        k.Schuelers.Where(s => s.SNr != k.KKlasprNavigation.SNr).ToList())
                        .OrDefault(fkr, 0.2f);
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
            foreach (var klasse in db.Klassens.Where(k => k.Schuelers.Any() && k.Stundens.Any()))
            {
                var schueler = klasse.Schuelers.ToList();
                var pruefGegenstaende = klasse.Stundens.GroupBy(s => s.StGegenstand).Select(g => g.FirstOrDefault()).ToList();
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