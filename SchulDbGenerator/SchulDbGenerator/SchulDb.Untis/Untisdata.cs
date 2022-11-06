using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CsvHelper;

namespace SchulDb.Untis
{
    /// <summary>
    /// Lädt die CSV Dateien des Untistools in den Speicher.
    /// </summary>
    public class Untisdata
    {
        public static string[] Wochentage { get; } = new string[] { "MO", "DI", "MI", "DO", "FR", "SA", "SO" };
        public Adresse Adressen { get; private set; }
        public List<Fach> Faecher { get; private set; }
        public List<Klasse> Klassen { get; private set; }
        public List<Lehrer> Lehrer { get; private set; }
        public List<Raum> Raeume { get; private set; }
        public List<Unterricht> Stundenplan { get; private set; }
        private Untisdata() { }
        public static async Task<Untisdata> Load(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Untisdata untisdata = new Untisdata();
            untisdata.Adressen = await ReadJsonFile<Adresse>($"{path}/adressen.json");
            untisdata.Faecher = ReadUntisFile<Fach>($"{path}/Faecher.csv");
            untisdata.Klassen = ReadUntisFile<Klasse>($"{path}/Klassen.csv");
            untisdata.Lehrer = ReadUntisFile<Lehrer>($"{path}/Lehrer.csv");
            untisdata.Raeume = ReadUntisFile<Raum>($"{path}/Raeume.csv");
            untisdata.Stundenplan = ReadUntisFile<Unterricht>($"{path}/Stundenplan.csv");
            return untisdata;
        }
        private static List<T> ReadUntisFile<T>(string filename)
        {
            var configuration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ";"
            };

            try
            {
                using (var reader = new StreamReader(filename, Encoding.GetEncoding(1252)))
                using (var csv = new CsvReader(reader, configuration))
                {
                    return csv.GetRecords<T>().ToList();
                }
            }
            catch (Exception e)
            {
                throw new SchulDbException("Fehler beim Lesen der Untisdateien. ist der Pfad richtig?", e.InnerException);
            }

        }
        private static async Task<T> ReadJsonFile<T>(string filename)
        {
            try
            {
                using (var reader = File.OpenRead(filename))
                {
                    var data = await JsonSerializer.DeserializeAsync<T>(reader);
                    return data;
                }
            }
            catch (Exception e)
            {
                throw new SchulDbException("Fehler beim Lesen der JSON Adressdaten. Fehlt die Datei adressen.json?", e.InnerException);
            }
        }
    }

}