using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tankstelle
{
    [Table("Tag")]
    class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime Datum { get; set; }
        public int Wochentag { get; set; }
        public int Monat { get; set; }
        public int Jahr { get; set; }
    }
    [Table("Tankstelle")]
    class Tankstelle
    {
        public int Id { get; set; }
        public string Adresse { get; set; }
        public int Plz { get; set; }
        public string Ort { get; set; }
        public virtual ICollection<Preis> Preise { get; set; }
    }

    [Table("Kategorie")]
    class Kategorie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Preis> Preise { get; set; }
    }

    [Table("Preis")]
    class Preis
    {
        public int Id { get; set; }
        [Column(TypeName = "NUMERIC(9,4)")]
        public decimal Wert { get; set; }
        [Column(TypeName = "DATE")]
        public DateTime GueltigVon { get; set; }
        [Column(TypeName = "DATE")]
        public DateTime? GueltigBis { get; set; }
        [Required]
        public virtual Tankstelle Tankstelle { get; set; }
        [Required]
        public virtual Kategorie Kategorie { get; set; }
    }

    class TankstelleContext : DbContext
    {
        public TankstelleContext(DbContextOptions options) : base(options)
        {
        }

        public TankstelleContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=Tankstellen;");
            optionsBuilder.UseSqlite(@"Data Source=Tankstellen.db");
        }

        public DbSet<Tag> Tage { get; set; }
        public DbSet<Tankstelle> Tankstellen { get; set; }
        public DbSet<Kategorie> Kategorien { get; set; }
        public DbSet<Preis> Preise { get; set; }
    }
}
