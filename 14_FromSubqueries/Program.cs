using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchulDb.Model;

namespace FromSubqueries
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
Nutzen Sie die erstellte View *vAbteilungsstatistik* in den folgenden Abfragen. Achtung: Die korrekten
Lösungen zeigen nur die ersten 10 bzw. 20 Datensätze an.".Write();

            @"
Welche Abteilung hat mehr als 200 Schüler? Ordnen Sie nach der Schüleranzahl absteigend.".WriteItem();
            (from a in db.Abteilungens
             let schuelerGesamt = a.Klassens.Sum(k => k.Schuelers.Count)
             where schuelerGesamt > 200
             select new
             {
                 a.AbtNr,
                 a.AbtName,
                 SchuelerGesamt = schuelerGesamt
             }).Take(10).WriteMarkdown();

            @"
Welche Abteilung hat mehr als 27 Schüler pro Klasse (im Durchschnitt,
also Schüleranzahl / Klassenanzahl)? Achten Sie auf die Kommastellen in der Berechnung.".WriteItem();
            (from a in db.Abteilungens.Include(a=>a.Klassens).ThenInclude(k=>k.Schuelers).ToList()
             let avgAnz = (decimal)a.Klassens.Sum(k => k.Schuelers.Count) / a.Klassens.Count()
             where avgAnz > 27
             select new
             {
                 a.AbtNr,
                 SchuelerProKlasse = Math.Round(avgAnz, 2)
             }).Take(10).WriteMarkdown();


            @"
Für die folgenden Beispiele erstellen Sie eine View *vPruefungsstatistik* mit folgenden Spalten:

| P_Pruefer | L_Name | L_Vorname | P_Gegenstand | P_Note | S_Nr | S_Zuname | S_Vorname | K_Nr | K_Abteilung |
| --------- | ------ | --------- | ------------ | ------ | ---- | -------- | --------- | ---- | ----------- |

Erstellen Sie eine View *vNotenspiegel*, die ausgehend von *vPruefungsstatistik* die Anzahl
der vergebenen Noten pro Lehrer und Fach ermittelt. Die Spalte *KeineNote* zählt alle mit NULL
eingetragenen Prüfungen. Die Spalte *Beurteilt* gibt die Anzahl der beurteilten Prüfungen (Note ist
nicht NULL) an. Beantworten Sie mit Hilfe der View *vNotenspiegel* die folgenden Beispiele.".Write();
            (from p in db.Pruefungens.Include(p=>p.PPrueferNavigation).ToList()
             group p by new { p.PPrueferNavigation, p.PGegenstand } into g
             orderby g.Key.PPrueferNavigation.LNr, g.Key.PGegenstand
             select new
             {
                 P_Pruefer = g.Key.PPrueferNavigation.LNr,
                 L_Name = g.Key.PPrueferNavigation.LName,
                 L_Vorname = g.Key.PPrueferNavigation.LVorname,
                 P_Gegenstand = g.Key.PGegenstand,
                 Note1 = g.Count(pr=>pr.PNote == 1),
                 Note2 = g.Count(pr=>pr.PNote == 2),
                 Note3 = g.Count(pr=>pr.PNote == 3),
                 Note4 = g.Count(pr=>pr.PNote == 4),
                 Note5 = g.Count(pr=>pr.PNote == 5),
                 KeineNote = g.Count(pr=>pr.PNote == null),
                 Beurteilt = g.Count(pr=>pr.PNote != null),
             }).Take(20).WriteMarkdown();

            @"
Wie viele Prüfungen gab es pro Prüfer?".WriteItem();
            (from l in db.Lehrers
             orderby l.LNr
             select new
             {
                 Pruefer = l.LNr,
                 l.LName,
                 l.LVorname,
                 AnzPruefungen = l.Pruefungens.Count()
             }).Take(10).WriteMarkdown();

            @"
Wie viel Prozent negative Prüfungen (Prüfungen mit 5 in Relation zu den beurteilten Prüfungen)
gab es bei jedem Lehrer pro Gegenstand?".WriteItem();
            (from p in db.Pruefungens.ToList()
             group p by new { p.PPruefer, p.PGegenstand } into g
             orderby g.Key.PPruefer, g.Key.PGegenstand
             select new
             {
                 g.Key.PPruefer,
                 g.Key.PGegenstand,
                 ProzentNegativ = Math.Round(100M * g.Count(pr => pr.PNote == 5) / Math.Max(1, g.Count(pr => pr.PNote != null)), 2)
             }).Take(20).WriteMarkdown();

            @"
Wie viel Prozent negative Prüfungen (Prüfungen mit 5 in Relation zu den beurteilten Prüfungen)
gab es bei jedem Lehrer über alle Gegenstände hinweg? Summieren Sie mit einer Unterabfrage
in FROM vorher die Prüfungen des Prüfers in *vNotenspiegel* in allen seinen Gegenständen auf.".WriteItem();
            (from p in db.Pruefungens.ToList()
             group p by new { p.PPruefer } into g
             orderby g.Key.PPruefer
             select new
             {
                 g.Key.PPruefer,
                 ProzentNegativ = Math.Round(100M * g.Count(pr => pr.PNote == 5) / Math.Max(1, g.Count(pr => pr.PNote != null)), 2)
             }).Take(10).WriteMarkdown();

        }
    }
}
