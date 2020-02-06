using System;
using System.Collections.Generic;
using System.Text;

namespace SchulDb.Untis
{
    public class Ort
    {
        public int Plz { get; set; }
        public string Name { get; set; }
        public string Bundesland { get; set; }
    }

    public class Strasse
    {
        public string Name { get; set; }
    }
    public class Adresse
    {
        public List<Ort> Orte { get; set; }
        public List<Strasse> Strassen { get; set; }
    }

}
