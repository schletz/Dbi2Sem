using System.ComponentModel.DataAnnotations.Schema;

namespace Lieferservice
{
    [Table("ProduktBestellung")]
    public class ProduktBestellung
    {
        public int BestellungId { get; set; }
        public Bestellung Bestellung { get; set; } = default!;
        public int ProduktId { get; set; }
        public Produkt Produkt { get; set; } = default!;
        public int Menge { get; set; }
    }
        
}
