using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchulDb.Model;

namespace SingleValueCorresponding
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<SchuleContext>()
                .UseSqlite("DataSource=../Schule.db")
                .Options;
            using var db = new SchuleContext(options);

            @"
Geben Sie die Klassen der Abteilung HIF und die Anzahl der männlichen und weiblichen Schüler aus.".WriteItem();
            (from k in db.Klassens
             where k.KAbteilung == "HIF"
             orderby k.KNr
             select new
             {
                 Klasse = k.KNr,
                 AnzGesamt = k.Schuelers.Count(),
                 AnzMaennl = k.Schuelers.Where(k => k.SGeschlecht == 1).Count(),
                 AnzWeibl = k.Schuelers.Where(k => k.SGeschlecht == 2).Count()
             }).WriteMarkdown();

            @"
In welchen Klassen gibt es mehr als doppelt so viel weibliche wie männliche Schüler?".WriteItem();
            (from k in db.Klassens
             let anzMaennl = k.Schuelers.Where(k => k.SGeschlecht == 1).Count()
             let anzWeibl = k.Schuelers.Where(k => k.SGeschlecht == 2).Count()
             where anzWeibl > 2 * anzMaennl
             orderby k.KNr
             select new
             {
                 Klasse = k.KNr,
                 AnzGesamt = k.Schuelers.Count(),
                 AnzMaennl = anzMaennl,
                 AnzWeibl = anzWeibl
             }).WriteMarkdown();

            @"Wie viele Stunden pro Woche sehen die Klassen der Abteilung AIF ihren Klassenvorstand? Lösen Sie
das Beispiel zuerst mit einem klassischen JOIN in Kombination mit einer Gruppierung. Danach lösen Sie
das Beispiel mit einer Unterabfrage ohne JOIN. Betrachten Sie nur Klassen mit eingetragenem Klassenvorstand.".WriteItem();
            (from k in db.Klassens
             let anzKvStunden = k.Stundens.Where(s => s.StLehrer == k.KVorstand).Count()
             where k.KVorstand != null && k.KAbteilung == "AIF"
             orderby k.KNr
             select new
             {
                 k.KNr,
                 AnzKvStunden = anzKvStunden
             }).WriteMarkdown();

            @"Wie viele Wochenstunden haben die Klassen der Abteilung AIF? Achtung: Es gibt Stunden, in denen
2 Lehrer in der Klasse sind. Pro Tag und Stunde ist jeder Datensatz nur 1x zu zählen. Könnten Sie
das Beispiel auch mit einem JOIN und einer Gruppierung lösen? Begründen Sie, wenn nicht.
Anmerkung, die nichts mit der Abfrage zu tun hat: Durch Stundenverlegungen können unterschiedliche
Werte bei Parallelklassen entstehen.".WriteItem();
            (from k in db.Klassens.Include(k => k.Stundens).ToList()
             where k.KAbteilung == "AIF"
             select new
             {
                 k.KNr,
                 AnzDatensaetze = k.Stundens.Count(),
                 AnzStunden = k.Stundens.GroupBy(s => new { s.StTag, s.StStunde }).Count()
             }).WriteMarkdown();

            @"
Wie viel Prozent der Stunden verbringen die Schüler der Abteilung KKU (Kolleg für Design) in ihrem
Stammraum? Für diese Anzahl werden einfach die Anzahl der Datensätze in der Stundentabelle gezählt.".WriteItem();
            (from k in db.Klassens
             let anzStunden = k.Stundens.Count()
             let anzStundenStammraum = k.Stundens.Count(s => s.StRaum == k.KStammraum)
             where k.KStammraum != null && k.KAbteilung == "KKU"
             orderby k.KNr
             select new
             {
                 k.KNr,
                 k.KStammraum,
                 AnzStundenGesamt = anzStunden,
                 AnzStundenStammraum = anzStundenStammraum,
                 ProzentImStammraum = Math.Round(100M * anzStundenStammraum / anzStunden, 0)
             }).WriteMarkdown();

            @"Welche Lehrer verdienen 50% mehr als der Durchschnitt von den Lehrern, die vorher in
die Schule eingetreten sind (Eintrittsjahr < Eintrittsjahr des Lehrers)?".WriteItem();
            var lehrerLocal = db.Lehrers.ToList();
            (from l in lehrerLocal
             let avgGehalt = lehrerLocal.Where(le => l.LEintrittsjahr < le.LEintrittsjahr).Average(le => le.LGehalt)
             where l.LGehalt > avgGehalt * 1.5M
             orderby l.LEintrittsjahr, l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname,
                 l.LGehalt,
                 l.LEintrittsjahr,
                 AvgGehaltAeltere = Math.Round(avgGehalt ?? 0, 2)
             }).WriteMarkdown();

            @"Welche Schüler haben im Gegenstand POS1 schlechtere Noten als der Durchschnitt der Prüfungen
bei diesem Prüfer in POS1?".WriteItem();
            (from p in db.Pruefungens
             let mittel = p.PPrueferNavigation.Pruefungens.Where(pr => pr.PGegenstand == "POS1").Average(pr => pr.PNote)
             where p.PNote > mittel && p.PGegenstand == "POS1"
             orderby p.PPruefer, p.PNote
             select new
             {
                 p.PKandidatNavigation.SNr,
                 p.PKandidatNavigation.SZuname,
                 p.PKandidatNavigation.SVorname,
                 p.PKandidatNavigation.SKlasse,
                 p.PPruefer,
                 p.PNote,
                 p.PGegenstand,
                 PrueferMittel = Math.Round(mittel ?? 0, 2)
             }).WriteMarkdown();

            @"Verallgemeinern Sie das vorige Beispiel auf beliebige Fächer: Welche Schüler der 1AHIF 
haben schlechtere Noten als der Prüfer im Mittel für diesen Gegenstand vergibt?".WriteItem();
            (from p in db.Pruefungens
             let mittel = p.PPrueferNavigation.Pruefungens.Where(pr => pr.PGegenstand == p.PGegenstand).Average(p => p.PNote)
             where p.PNote > mittel && p.PKandidatNavigation.SKlasse == "1AHIF"
             orderby p.PPruefer, p.PNote
             select new
             {
                 p.PKandidatNavigation.SNr,
                 p.PKandidatNavigation.SZuname,
                 p.PKandidatNavigation.SVorname,
                 p.PKandidatNavigation.SKlasse,
                 p.PPruefer,
                 p.PNote,
                 p.PGegenstand,
                 PrueferMittel = Math.Round(mittel ?? 0, 2)
             }).WriteMarkdown();

            @"Geben Sie die letzte Stunde der 3BAIF für jeden Wochentag aus. Beachten Sie, dass
auch mehrere Datensätze für die letzte Stunde geliefert werden können (wenn 2 Lehrer dort unterrichten).".WriteItem();
            (from s in db.Stundens
             let letzteStunde = db.Stundens.Where(st => st.StKlasse == s.StKlasse && st.StTag == s.StTag).Max(st => st.StStunde)
             where s.StKlasse == "3BAIF" && s.StStunde == letzteStunde && s.StKlasseNavigation.KAbteilung == "AIF"
             orderby s.StKlasse, s.StTag
             select new
             {
                 s.StKlasse,
                 s.StTag,
                 s.StStunde,
                 s.StGegenstand,
                 s.StLehrer
             }).WriteMarkdown();

        }
    }
}
