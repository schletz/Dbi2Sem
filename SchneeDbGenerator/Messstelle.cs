using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchneeDbGenerator
{
    [Index("M_Skigebiet", Name = "skigebieteMessstellen")]
    public class Messstelle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int M_Id { get; set; }
        public int? M_Skigebiet { get; set; }
        public short? M_Seehoehe { get; set; }
        public int? M_GPS_X { get; set; }
        public int? M_GPS_Y { get; set; }

        [ForeignKey("M_Skigebiet")]
        [InverseProperty("Messstellen")]
        [CsvHelper.Configuration.Attributes.Ignore]
        public virtual Skigebiete M_SkigebietNavigation { get; set; } = default!;
        [InverseProperty("Sm_MessstelleNavigation")]
        public virtual ICollection<Schneemessung> Schneemessung { get; set; } = default!;
    }
}