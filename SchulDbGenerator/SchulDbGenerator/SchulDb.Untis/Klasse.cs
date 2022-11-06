namespace SchulDb.Untis
{
    public class Klasse
    {
        public string Nr { get; set; }
        public string Beschreibung { get; set; }
        public string Flag1 { get; set; }
        public string Stammraum { get; set; }
        public string Flag2 { get; set; }
        public string Flag3 { get; set; }
        public string Flag4 { get; set; }
        public string Flag5 { get; set; }
        public string Flag6 { get; set; }
        public string Flag7 { get; set; }
        public string Flag8 { get; set; }
        public string Flag9 { get; set; }
        public string Flag10 { get; set; }
        public int? Schulstufe { get; set; }
        public string Abteilung { get; set; }
        public string Flag13 { get; set; }
        public int? SchuelerMaennl { get; set; }
        public int? SchuelerWeibl { get; set; }
        public int? Schulform { get; set; }
        public int? DatumVon { get; set; }
        public int? DatumBis { get; set; }
        public string Text { get; set; }
        public string Flag20 { get; set; }
        public string Flag21 { get; set; }
        public string Flag22 { get; set; }
        public string Flag23 { get; set; }
        public string Flag24 { get; set; }
        public string Flag25 { get; set; }
        public string Flag26 { get; set; }
        public string Kv { get; set; }
        public string Flag28 { get; set; }
        public string Flag29 { get; set; }
        public string Flag30 { get; set; }
        public int Jahresform
        {
            get
            {
                try
                {
                    if (Nr[2] == 'O') { return 0; }
                    if (Nr[2] == 'H') { return 0; }
                    if (Nr[2] == 'F') { return 0; }
                    if (int.Parse(Nr.Substring(0, 1)) % 2 == 0) { return 2; }
                    else { return 1; }
                }
                catch { }
                return 0;
            }
        }
        public float PercentFemale
        {
            get
            {
                try
                {
                    if (Nr.Substring(1, 4) == "AHIF") { return 0.2f; }
                    if (Nr.Substring(1, 4) == "EHIF") { return 0.2f; }
                    if (Nr.Substring(2, 3) == "HIF") { return 0f; }
                }
                catch { }
                return 0.5f;
            }
        }

        public bool Abschlussklasse
        {
            get
            {
                try
                {
                    if (Nr[0] == '5' && Nr[2] == 'H') { return true; }
                    if (Nr[0] == '4' && Nr[2] == 'F') { return true; }
                    if (Nr[0] == '8' && Nr[2] == 'B') { return true; }
                    if (Nr[0] == '8' && Nr[2] == 'C') { return true; }
                }
                catch { }
                return false;
            }
        }
    }
}
