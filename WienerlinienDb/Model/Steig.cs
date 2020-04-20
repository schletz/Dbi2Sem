namespace WienerlinienDb.Model
{
    public class Steig
    {
        public int SteigId { get; set; }
        public Linie Linie { get; set; }
        public Haltestelle Haltestelle { get; set; }
        public string Name { get; set; }
        public string Richtung { get; set; }
        public int Reihenfolge { get; set; }
        public decimal Wgs84_Lat { get; set; }
        public decimal Wgs84_Lon { get; set; }
    }
}
