using System;
using System.Collections.Generic;
using System.Text;

namespace WienerlinienDb.Model
{
    public class Fahrt
    {
        public int FahrtId { get; set; }
        public DateTime Fahrtantritt { get; set; }
        public Haltestelle Einstieg { get; set; } = default!;
        public Haltestelle Ausstieg { get; set; } = default!;
    }
}
