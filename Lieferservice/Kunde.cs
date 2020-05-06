using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lieferservice
{
    [Table("Kunde")]
    public class Kunde
    {
        public int KundeId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Vorname { get; set; }
        [Required]
        [MaxLength(255)]
        public string Zuname { get; set; }
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        public List<Bestellung> Bestellungen { get; set; }
    }

}
