using System.ComponentModel.DataAnnotations.Schema;

namespace Lieferservice
{
    [Table("ProduktBestellung")]
    public class ProduktBestellung
    {
        public Bestellung Bestellung { get; set; } = default!;
        public Produkt Produkt { get; set; } = default!;
        public int Menge { get; set; }
    }
        
}
