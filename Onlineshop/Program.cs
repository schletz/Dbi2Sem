using Onlineshop.Model;
using System;

namespace Onlineshop
{
    class Program
    {
        static void Main(string[] args)
        {
            using (OnlineshopContext db = new OnlineshopContext())
            {
                db.Database.EnsureDeleted();
                if (db.Database.EnsureCreated()) { db.Seed(); }
            }
        }
    }
}
