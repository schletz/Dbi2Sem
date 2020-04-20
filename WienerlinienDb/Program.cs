using System;
using WienerlinienDb.Model;

namespace WienerlinienDb
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WienerlinienContext db = new WienerlinienContext())
            {
                db.Database.EnsureDeleted();
                if (db.Database.EnsureCreated())
                {
                    db.Seed();
                }
            }

        }
    }
}
