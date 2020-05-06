using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lieferservice
{
    [Table("Liefergebiet")]
    public class Liefergebiet
    {
        public int Plz { get; set; }
        [MaxLength(255)]
        public string Ort { get; set; }
        public List<Bestellung> Bestellungen { get; set; }
        [DataType("DECIMAL(4,2)")]
        public decimal Lieferzuschlag { get; set; }
    }

}
