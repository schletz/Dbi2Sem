using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace WienerlinienDb.Csv
{
    public class Datareader
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public class Haltestelle
        {
            public int Haltestellen_Id { get; set; }
            public string Name { get; set; }
            public string Gemeinde { get; set; }
            public int Gemeinde_Id { get; set; }
            public decimal? Wgs84_Lat { get; set; }
            public decimal? Wgs84_Lon { get; set; }
        }

        public class Linie
        {
            public int Linien_Id { get; set; }
            public string Bezeichnung { get; set; }
            public int Reihenfolge { get; set; }
            public string Verkehrsmittel { get; set; }
        }

        public class Steig
        {
            public int Steig_Id { get; set; }
            public int Fk_Linien_Id { get; set; }
            public int Fk_Haltestellen_Id { get; set; }
            public string Richtung { get; set; }
            public int Reihenfolge { get; set; }
            [Name("Steig")]
            public string Name { get; set; }
            public decimal Steig_Wgs84_Lat { get; set; }
            public decimal Steig_Wgs84_Lon { get; set; }
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public static List<T> ReadFile<T>(string filename)
        {
            try
            {
                using (var reader = new StreamReader(filename, Encoding.UTF8))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                    csv.Configuration.Delimiter = ";";
                    return csv.GetRecords<T>().ToList();
                }
            }
            catch
            {
                throw;
            }

        }
    }
}
