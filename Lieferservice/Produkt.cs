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
        public string Name { get; set; } = default!;
        public List<ProduktBestellung> ProduktBestellungen { get; set; } = default!;
        [Required]
        public Kategorie Kategorie { get; set; } = default!;
        public decimal Preis { get; internal set; }
    }

}
