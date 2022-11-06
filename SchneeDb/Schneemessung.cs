using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchneeDbGenerator
{
    [Index("Sm_Messstelle", Name = "MessstellenSchneemessung")]
    public class Schneemessung
    {
        public int Sm_Messstelle { get; set; }
        public DateTime Sm_DatumZeit { get; set; }
        public short? Sm_schnee { get; set; }

        [ForeignKey("Sm_Messstelle")]
        [InverseProperty("Schneemessung")]
        [CsvHelper.Configuration.Attributes.Ignore]

        public virtual Messstelle Sm_MessstelleNavigation { get; set; } = default!;
    }
}