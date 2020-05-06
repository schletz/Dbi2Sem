using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lieferservice
{
    [Table("Produkt")]
    public class Produkt
    {
        public int ProduktId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public List<ProduktBestellung> ProduktBestellungen { get; set; }
        [Required]
        public Kategorie Kategorie { get; set; }
        public decimal Preis { get; internal set; }
    }

}
