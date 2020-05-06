using System.ComponentModel.DataAnnotations.Schema;

namespace Lieferservice
{
    [Table("ProduktBestellung")]
    public class ProduktBestellung
    {
        public Bestellung Bestellung { get; set; }
        public Produkt Produkt { get; set; }
        public int Menge { get; set; }
    }
        
}
