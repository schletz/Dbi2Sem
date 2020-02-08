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
In welchen Klassen der Abteilung HIF kommt das Fach NW2 nicht im Stundenplan vor? Hinweis:
Arbeiten Sie mit der Menge der Klassen, in denen NW2 unterrichtet wird.".WriteItem();
            (from k in db.Klassens
             where k.KAbteilung == "HIF" && !k.Stundens.Any(k => k.StGegenstand == "NW2")
             orderby k.KNr
             select new
             {
                 k.KNr
             }).WriteMarkdown();

            @"
Welche Gegenstände werden gar nicht geprüft? Lösen Sie die Aufgabe mit einem LEFT JOIN und danach
mit einer Unterabfrage. Hinweis: Arbeiten Sie mit der Menge der Gegenstände, die in der
Prüfungstabelle eingetragen sind.".WriteItem();

            (from g in db.Gegenstaendes
             where !g.Pruefungens.Any()
             select new
             {
                 g.GNr,
                 g.GBez
             }).WriteMarkdown();

            @"
Welche Gegenstände werden nur praktisch geprüft (*P_Art* ist p)? Können Sie die Aufgabe auch mit
LEFT JOIN lösen? Begründen Sie wenn nicht. Hinweis: Arbeiten Sie mit der Menge der Gegenstände,
die NICHT praktisch geprüft werden. Betrachten Sie außerdem nur Gegenstände, die überhaupt geprüft
werden. Würden Gegenstände, die gar nicht geprüft werden, sonst aufscheinen? Macht das einen
(aussagenlogischen) Sinn?".WriteItem();
            (from g in db.Gegenstaendes
             where g.Pruefungens.Any() && !g.Pruefungens.Any(p => p.PArt != "p")
             orderby g.GNr
             select new
             {
                 g.GNr,
                 g.GBez
             }).WriteMarkdown();

            @"
Gibt es Prüfungen im Fach BWM, die von Lehrern abgenommen wurden, die die Klasse gar nicht
unterrichten? Hinweis: Arbeiten Sie über die Menge der Lehrer, die den angezeigten Schüler unterrichten.".WriteItem();
            (from p in db.Pruefungens
             where p.PGegenstand == "BWM" && !p.PPrueferNavigation.Stundens.Any(s => s.StKlasse == p.PKandidatNavigation.SKlasse)
             orderby p.PPruefer, p.PDatumZeit
             select new
             {
                 p.PPruefer,
                 p.PDatumZeit,
                 p.PKandidatNavigation.SNr,
                 p.PKandidatNavigation.SZuname,
                 p.PKandidatNavigation.SVorname,
                 p.PGegenstand,
                 p.PNote
             }).WriteMarkdown();

            @"
Für die Maturaaufsicht in POS werden Lehrer benötigt, die zwar in POS (Filtern nach POS%) unterrichten,
aber in keiner 5. HIF Klasse (*K_Schulstufe* ist 13 und *K_Abteilung* ist HIF) sind.".WriteItem();
            (from l in db.Lehrers
             where
                l.Stundens.Any(s => s.StGegenstand.StartsWith("POS")) &&
                !l.Stundens.Any(s =>
                    s.StKlasseNavigation.KSchulstufe == 13 &&
                    s.StKlasseNavigation.KAbteilung == "HIF")
             orderby l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname
             }).WriteMarkdown();

            @"
Lösen Sie das vorige Beispiel mit anderen Bedingungen: Geben Sie die Lehrer aus, die weder in einer
5. Klasse (*K_Schulstufe* ist 13) noch in einer HIF Klasse (*K_Abteilung* ist HIF) unterrichten.
Wie ändert sich Ihre Abfrage?".WriteItem();
            (from l in db.Lehrers
             where
                l.Stundens.Any(s => s.StGegenstand.StartsWith("POS")) &&
                !l.Stundens.Any(s => s.StKlasseNavigation.KSchulstufe == 13) &&
                !l.Stundens.Any(s => s.StKlasseNavigation.KAbteilung == "HIF")
             orderby l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname
             }).WriteMarkdown();

            @"
Welche Klassen der HIF Abteilung haben auch in den Abendstunden (*Stundenraster.Str_IstAbend* = 1)
Unterricht?".WriteItem();
            (from k in db.Klassens
             where k.KAbteilung == "HIF" && k.Stundens.Any(s => s.StStundeNavigation.StrIstAbend)
             orderby k.KNr
             select new
             {
                 k.KNr
             }).WriteMarkdown();



            @"
Welche Lehrer haben Montag und Freitag frei, also keinen Unterricht an diesen Tagen in der
Stundentabelle? Anmerkung, die nichts mit der Lösung zu tun hat: Religion und die Freifächer
wurden - in Abweichung zu den Realdaten - nicht importiert.".WriteItem();
            (from l in db.Lehrers
             where !l.Stundens.Any(s => s.StTag == 1 || s.StTag == 5)
             orderby l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname
             }).WriteMarkdown();

            @"
Wie das vorherige Beispiel, allerdings sollen nur Lehrer, die auch Stunden haben (also in der
Tabelle Stunden überhaupt vorkommen), berücksichtigt werden? Anmerkung, die nichts mit der Lösung
zu tun hat: Religion und die Freifächer wurden  - in Abweichung zu den Realdaten - nicht importiert.".WriteItem();
            (from l in db.Lehrers
             where l.Stundens.Any() && !l.Stundens.Any(s => s.StTag == 1 || s.StTag == 5)
             orderby l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname
             }).WriteMarkdown();

            @"
Schwer, sozusagen ein SQL Hyperstar Problem: Welche Klassenvorstände unterrichten nur
in Abteilungen, die auch der Klasse
entsprechen, von der sie Klassenvorstand sind? Diese Abfrage hat eine besondere Schwierigkeit: Da
Lehrer auch von mehreren Klassen Klassenvorstand sein können, die in verschiedenen Abteilungen sein
können (z. B. Tag und Abend) brauchen Sie hier geschachtelte Unterabfragen.

1. Das Problem ist durch eine Negierung zu lösen, da IN den Existenzquantor darstellt, und wir hier
   einen Allquantor brauchen.
2. Finden Sie zuerst heraus, in welchen Abteilungen der Lehrer KV ist.
3. Finden Sie die Lehrer heraus, die nicht in der Liste der Abteilungen aus (2) unterrichten.
4. Der Lehrer darf nicht in der Liste von (3) vorkommen.
5. Betrachten Sie zum Schluss nur die Lehrer, die auch KV sind. Lehrer, die kein KV sind, würden
   nämlich aussagenlogisch auch nur in Abteilungen unterrichten, von denen sie KV sind.
   
Korrekte Ausgabe:".WriteItem();
            (from l in db.Lehrers
             where
                l.Klassens.Any() &&
                !l.Stundens.Any(s => !l.Klassens.Any(k => k.KAbteilung == s.StKlasseNavigation.KAbteilung))
             orderby l.LNr
             select new
             {
                 l.LNr,
                 l.LName,
                 l.LVorname
             }).WriteMarkdown();
        }
    }
}
