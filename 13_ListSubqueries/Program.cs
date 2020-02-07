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
Welche Lehrer haben Montag und Freitag frei, also keinen Unterricht an diesen Tagen in der
Stundentabelle? Anmerkung, die nichts mit der Lösung zu tun hat: Religion und die Freifächer wurden
- in Abweichung zu den Realdaten - nicht importiert.".WriteItem();
            (from l in db.Lehrers
             where !l.Stundens.Any(s=>s.StTag == 1 || s.StTag == 5)
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
             where l.Stundens.Any() && !l.Stundens.Any(s=>s.StTag == 1 || s.StTag == 5)
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
