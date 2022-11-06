using System.Collections.Generic;

namespace WienerlinienDb.Model
{
    public class Haltestelle
    {
        public int HaltestelleId { get; set; }
        public string Name { get; set; } = default!;
        public string Gemeinde { get; set; } = default!;
        public int? Gemeindebezirk { get; set; }
        public int Gemeinde_Id { get; set; }
        public decimal? Wgs84_Lat { get; set; }
        public decimal? Wgs84_Lon { get; set; }
        public List<Steig> Steige { get; set; } = default!;
    }
}
