using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lieferservice
{
    [Table("Bestellung")]
    public class Bestellung
    {
        public int BestellungId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Adresse { get; set; } = default!;
        [Required]
        public Liefergebiet Liefergebiet { get; set; } = default!;
        [Required]
        public Kunde Kunde { get; set; } = default!;
        public DateTime Bestellzeit { get; set; }
        [Required]
        public List<ProduktBestellung> ProduktBestellungen { get; set; } = default!;
    }
        
}
