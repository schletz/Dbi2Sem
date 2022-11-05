using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lieferservice
{
    [Table("Kategorie")]
    public class Kategorie
    {
        public int KategorieId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = default!;
        public List<Produkt> Produkte { get; set; } = default!;
    }

}
