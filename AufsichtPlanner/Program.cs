using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

//var opt = new DbContextOptionsBuilder()
//    .UseSqlite("Data Source=Aufsicht.db")
//    .Options;
var dbName = "Aufsicht";
var opt = new DbContextOptionsBuilder()
    .UseOracle($"User Id={dbName};Password=oracle;Data Source=localhost:1521/XEPDB1")
    .Options;

var sysOpt = new DbContextOptionsBuilder()
    .UseOracle($"User Id=System;Password=oracle;Data Source=localhost:1521/XEPDB1")
    .Options;
try
{
    using (var sysDb = new AufsichtContext(sysOpt))
    {
        try { sysDb.Database.ExecuteSqlRaw("DROP USER " + dbName + " CASCADE"); }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }
        sysDb.Database.ExecuteSqlRaw("CREATE USER " + dbName + " IDENTIFIED BY oracle");
        sysDb.Database.ExecuteSqlRaw("GRANT CONNECT, RESOURCE, CREATE VIEW TO " + dbName);
        sysDb.Database.ExecuteSqlRaw("GRANT UNLIMITED TABLESPACE TO " + dbName);
    }
    Console.WriteLine("*********************************************************");
    Console.WriteLine("Du kannst dich nun mit folgenden Daten verbinden:");
    Console.WriteLine($"   Username:     {dbName}");
    Console.WriteLine($"   Passwort:     oracle");
    Console.WriteLine($"   Service Name: XEPDB1");
    Console.WriteLine("*********************************************************");
}
catch (Exception e)
{
    Console.Error.WriteLine("Fehler beim Löschen und neu Anlegen des Oracle Benutzers.");
    Console.Error.WriteLine("Fehlermeldung: " + e.Message);
    return;
}

using var db = new AufsichtContext(opt);
db.Database.EnsureDeleted();
db.Database.EnsureCreated();
db.Seed();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class Teacher
{
    [Key]
    public string Shortname { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class Subject
{
    [Key]
    public string Shortname { get; set; }
    public string Name { get; set; }
}
public class Room
{
    [Key]
    public string Shortname { get; set; }
    public int Capacity { get; set; }
}

public class Lesson
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Number { get; set; }
    public TimeSpan Begin { get; set; }
}
public class Supervision
{
    public int Id { get; set; }
    [Column("Lesson_Number")]
    public int LessonNumber { get; set; }
    public Lesson Lesson { get; set; }
    [Column("Teacher_Shortname")]
    public string TeacherShortname { get; set; }
    public Teacher Teacher { get; set; }
    [Column("Room_Shortname")]
    public string RoomShortname { get; set; }
    public Room Room { get; set; }
    [Column("Subject_Shortname")]
    public string SubjectShortname { get; set; }
    public Subject Subject { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class AufsichtContext : DbContext
{
    public AufsichtContext(DbContextOptions opt) : base(opt)
    { }
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Supervision> Supervisions => Set<Supervision>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supervision>()
            .HasIndex(s => new { s.LessonNumber, s.RoomShortname, s.SubjectShortname })
            .IsUnique();
        // Für Oracle alle Namen großschreiben, sonst sind sie Case Sensitive und brauchen
        // ein " bei den Abfragen.
        if (Database.IsOracle())
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var schema = entity.GetSchema();
                var tableName = entity.GetTableName();
                if (tableName is null) { continue; }
                var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, schema);
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName(storeObjectIdentifier)?.ToUpper());
                }
                entity.SetTableName(tableName.ToUpper());
            }
        }
    }

    public void Seed()
    {
        Randomizer.Seed = new Random(1443);
        var faker = new Faker("de");
        var capacities = new int[] { 16, 24, 32 };
        var rooms = Enumerable.Range(6, 6).Select(i => $"B3.{i:00}")
            .Concat(Enumerable.Range(6, 6).Select(i => $"B4.{i:00}"))
            .Select(room => new Room { Shortname = room, Capacity = faker.Random.ListItem(capacities) })
            .ToList();
        Rooms.AddRange(rooms);
        SaveChanges();

        var subjects = new Subject[]
        {
            new Subject(){Shortname = "D", Name = "Deutsch"},
            new Subject(){Shortname = "E", Name = "Englisch"},
            new Subject(){Shortname = "AM", Name = "Angewandte Mathematik"},
            new Subject(){Shortname = "FT", Name = "Fachtheorie"}
        };
        Subjects.AddRange(subjects);
        SaveChanges();
        var durations = new Dictionary<string, int>()
        {
            {"D", 7 },
            {"E", 7 },
            {"AM", 8 },
            {"FT", 8 },
        };

        var lessons = new Lesson[] {
            new Lesson() {Number = 1, Begin = new TimeSpan(8,0,0) },
            new Lesson() {Number = 2, Begin = new TimeSpan(8,50,0) },
            new Lesson() {Number = 3, Begin = new TimeSpan(9,55,0) },
            new Lesson() {Number = 4, Begin = new TimeSpan(10,45,0) },
            new Lesson() {Number = 5, Begin = new TimeSpan(11,35,0) },
            new Lesson() {Number = 6, Begin = new TimeSpan(12,35,0) },
            new Lesson() {Number = 7, Begin = new TimeSpan(13,25,0) },
            new Lesson() {Number = 8, Begin = new TimeSpan(14,25,0) }
        };
        Lessons.AddRange(lessons);
        SaveChanges();

        var teachers = new Faker<Teacher>("de").CustomInstantiator(f =>
        {
            var name = f.Name.LastName();
            return new Teacher
            {
                Shortname = name.Substring(0, 3).ToUpper(),
                Firstname = f.Name.FirstName(),
                Lastname = name
            };
        })
        .Generate(100)
        .GroupBy(t => t.Shortname).Select(g => g.First())
        .Take(30)
        .ToList();
        Teachers.AddRange(teachers);
        SaveChanges();

        var roomsWithSupervision = faker.Random.ListItems(rooms, rooms.Count - 2);
        var teachersWithSupervision = faker.Random.ListItems(teachers, teachers.Count - 5);
        var supervisions = new List<Supervision>();
        foreach (var s in subjects)
        {
            var duration = durations[s.Shortname];
            var roomsWithSupervisionSubject = faker.Random.ListItems(roomsWithSupervision, roomsWithSupervision.Count - 2).ToList();
            foreach (var r in roomsWithSupervisionSubject)
            {
                for (int lessonNr = 0; lessonNr < duration;)
                {
                    int lessonCount = faker.Random.Int(1, 3);
                    for (int i = 0; i < lessonCount; i++)
                    {
                        if (i >= lessons.Length) { break; }
                        if (!faker.Random.Bool(0.9f)) { break; }
                        var lesson = lessons[lessonNr];
                        var teacher = faker.Random.ListItem(teachersWithSupervision);
                        supervisions.Add(new Supervision
                        {
                            Subject = s,
                            SubjectShortname = s.Shortname,
                            LessonNumber = lesson.Number,
                            Lesson = lesson,
                            Room = r,
                            RoomShortname = r.Shortname,
                            TeacherShortname = teacher.Shortname,
                            Teacher = teacher,
                        });
                    }
                    lessonNr += lessonCount;
                }
            }
        }

        supervisions = supervisions
            .GroupBy(s => new { s.LessonNumber, s.RoomShortname, s.SubjectShortname })
            .Select(s => s.First()).ToList();
        Supervisions.AddRange(supervisions);
        SaveChanges();
    }
}