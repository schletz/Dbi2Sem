using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchulDb.Model;

namespace SingleValueNonCorresponding
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<SchuleContext>()
                .UseSqlite("DataSource=../Schule.db")
                .Options;
            using var db = new SchuleContext(options);
            @"Welche Lehrer sind neu bei uns, haben also das maximale Eintrittsjahr?".WriteItem();
            (from l in db.Lehrers
             let maxJahr = db.Lehrers.Max(l => l.LEintrittsjahr)
             where l.LEintrittsjahr == maxJahr
             orderby l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname,
                 l.LEintrittsjahr
             }).WriteMarkdown();

            @"
Geben Sie die Klassen der Abteilung AIF und die Anzahl der gesamten Klassen und Schüler der Schule aus.".WriteItem();
            (from k in db.Klassens
             where k.KAbteilung == "AIF"
             orderby k.KNr
             select new
             {
                 Klasse = k.KNr,
                 KlassenGesamt = db.Klassens.Count(),
                 SchuelerGesamt = db.Schuelers.Count()
             }).WriteMarkdown();

            @"
Geben Sie allen Lehrern, die 2018 eingetreten sind (Spalte *L_Eintrittsjahr*), das Durchschnittsgehalt aus.".WriteItem();
            var avgLehrerGehalt = db.Lehrers.ToList().Average(l => l.LGehalt);
            (from l in db.Lehrers
             where l.LEintrittsjahr == 2018
             orderby l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname,
                 l.LEintrittsjahr,
                 l.LGehalt,
                 AvgGehalt = Math.Round(avgLehrerGehalt ?? 0, 2)
             }).WriteMarkdown();

            @"
Als Ergänzung geben Sie nun bei diesen Lehrern die Abweichung vom Durchschnittsgehalt
aus. Zeigen Sie dabei nur die Lehrer an, über 1000 Euro unter diesem Durchschnittswert verdienen.".WriteItem();
            (from l in db.Lehrers.ToList()
             let abw = l.LGehalt - avgLehrerGehalt
             where l.LEintrittsjahr == 2018 && abw < -1000
             orderby l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname,
                 l.LEintrittsjahr,
                 l.LGehalt,
                 AvgGehalt = Math.Round(avgLehrerGehalt ?? 0, 2),
                 Abweichung = Math.Round(abw ?? 0, 2)
             }).WriteMarkdown();

            @"
Geben Sie die Prüfungen aus, die maximal 3 Tage vor der letzten Prüfung stattfanden.".WriteItem();
            var letztePruef = db.Pruefungens.Max(p => p.PDatumZeit);
            (from p in db.Pruefungens.Include(p => p.PKandidatNavigation).ToList()
             where p.PDatumZeit >= letztePruef.AddDays(-3)
             orderby p.PDatumZeit descending
             select new
             {
                 p.PDatumZeit,
                 p.PPruefer,
                 p.PNote,
                 Zuname = p.PKandidatNavigation.SZuname,
                 Vorname = p.PKandidatNavigation.SVorname
             }).WriteMarkdown();

            @"
Geben Sie die Räume mit der meisten Kapazität (Spalte *R_Plaetze*) aus. Hinweis: Das können auch
mehrere Räume sein.".WriteItem();
            var maxPlaetze = db.Raeumes.Max(r => r.RPlaetze);
            (from r in db.Raeumes
             where r.RPlaetze == maxPlaetze
             orderby r.RId
             select r).WriteMarkdown();

            @"
Gibt es Räume, die unter einem Viertel der Plätze als der größte Raum haben?".WriteItem();
            (from r in db.Raeumes
             where r.RPlaetze < maxPlaetze / 4
             orderby r.RId
             select r).WriteMarkdown();

            @"
Welche Klasse hat mehr weibliche Schüler (S_Geschlecht ist 2) als die 5BAIF? Hinweis: Gruppieren Sie
die Schülertabelle und vergleichen die Anzahl mit dem ermittelten Wert aus der 5BAIF.".WriteItem();
            var anzWeibl5BAIF = db.Schuelers.Count(s => s.SKlasse == "5BAIF" && s.SGeschlecht == 2);
            (from k in db.Klassens
             let anzWeibl = k.Schuelers.Where(s => s.SGeschlecht == 2).Count()
             where anzWeibl > anzWeibl5BAIF
             select new
             {
                 Klasse = k.KNr,
                 AnzWeibl = anzWeibl
             }).WriteMarkdown();

            @"
Geben Sie die Klassen der Abteilung BIF sowie die Anzahl der Schüler in dieser Abteilung aus.
Hinweis: Verwenden Sie GROUP BY, um die Schüleranzahl pro Klasse zu ermitteln. Achten Sie auch
darauf, dass Klassen mit 0 Schülern auch angezeigt werden. Danach schreiben Sie 
eine Unterabfrage, die die Schüler der BIF Abteilung zählt.".WriteItem();
            (from k in db.Klassens
             where k.KAbteilung == "BIF"
             orderby k.KNr
             select new
             {
                 Klasse = k.KNr,
                 SchuelerKlasse = k.Schuelers.Count(),
                 SchuelerBIF = k.KAbteilungNavigation.Klassens.Sum(k => k.Schuelers.Count())
             }).WriteMarkdown();

        }
    }
}
