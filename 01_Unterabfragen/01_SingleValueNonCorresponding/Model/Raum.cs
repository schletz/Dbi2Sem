﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchulDbGenerator.Model
{
    [Table("Raum")]
    public partial class Raum
    {
        public Raum()
        {
            Stundens = new HashSet<Stunde>();
        }

        [Key]
        [Column("R_ID")]
        [StringLength(8)]
        public string RId { get; set; }
        [Column("R_Plaetze")]
        public int? RPlaetze { get; set; }
        [Column("R_Art")]
        public string RArt { get; set; }
        [InverseProperty(nameof(Stunde.StRaumNavigation))]
        public virtual ICollection<Stunde> Stundens { get; set; }

        [InverseProperty(nameof(Klasse.KStammraumNavigation))]
        public virtual ICollection<Klasse> Klassen { get; set; }
    }
}