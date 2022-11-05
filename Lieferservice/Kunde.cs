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
        public string Vorname { get; set; } = default!;
        [Required]
        [MaxLength(255)]
        public string Zuname { get; set; } = default!;
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = default!;
        public List<Bestellung> Bestellungen { get; set; } = default!;
    }

}
