using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchulDb.Model
{
    [Table("Stundenraster")]
    public class Stundenraster
    {
        [Key]
        [Column("Str_Nr")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]  // Für SQL Server, sonst glaubt EF Core es ist ein Autowert.
        public int StrNr { get; set; }
        [Column("Str_Beginn")]
        [Required]
        public TimeSpan StrBeginn { get; set; }
        [Column("Str_Ende")]
        [Required]
        public TimeSpan StrEnde { get; set; }
        [Column("Str_IstAbend")]
        public bool StrIstAbend { get; set; }
        [InverseProperty(nameof(Stunde.StStundeNavigation))]
        public ICollection<Stunde> Stundens { get; set; }
    }
}
