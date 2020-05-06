using System;

namespace Lieferservice
{
    class Program
    {
        static void Main(string[] args)
        {
            using(LieferserviceContext db = new LieferserviceContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.Seed();
            }
        }
    }
}
