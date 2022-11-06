namespace WienerlinienDb.Model
{
    public class Steig
    {
        public int SteigId { get; set; }
        public Linie Linie { get; set; } = default!;
        public Haltestelle Haltestelle { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Richtung { get; set; } = default!;
        public int Reihenfolge { get; set; }
        public decimal Wgs84_Lat { get; set; }
        public decimal Wgs84_Lon { get; set; }
    }
}
