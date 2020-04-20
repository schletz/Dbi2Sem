using System.Collections.Generic;

namespace WienerlinienDb.Model
{
    public class Linie
    {
        public int LinieId { get; set; }
        public string Name { get; set; }
        public int Reihenfolge { get; set; }
        public string Verkehrsmittel { get; set; }
        public List<Steig> Steige { get; set; }
    }
}
