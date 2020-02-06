# SchulDb Generator

Dieses Programm generiert - basierend auf den realen Untisdaten - Musterdaten für Schüler und
Prüfungen. Es verwendet EF Core und schreibt die gesamte Datenbank neu.

## Kopieren der Untisdaten

Für das Untistool werden täglich auf `\\enterprise\administration\schmidsUntisTool` csv Daten aus
der Untisdatenbank exportiert. Kopiere die folgenden Dateien in das Verzeichnis `Untis/Data`
dieser Applikation. Der String *2019-2020* kann im Programm noch angepasst werden.

- *AlleLehrer/Lehrer_2019-2020.csv*
- *DataFiles_2019-2020/\*.csv*

## Anpassen der Program.cs

### Anpassen der Ausgabedatenbank

Der DB Context wird im Hauptprogramm sehr allgemein konfiguriert. Es ist - nach dem Laden der
entsprechenden Provider - natürlich auch möglich, *UseSqlServer()*, ... zu verwenden.

```c#
var options = new DbContextOptionsBuilder<SchuleContext>()
    .UseSqlite("DataSource=Schule.db")
    .Options;
```

### Anpassen des Schuljahres

Im Programm gibt es einen auskommentierten Block, der die Datenbank generiert. Hier kann das Prefix
der Dateien und das Schuljahr, das als Basisdatum für zeitbasierende Musterdaten verwendet wird,
geändert werden.

```c#
Untisdata data = await Untisdata.Load("Untis/Data", "2019-2020");
using (SchuleContext db = new SchuleContext(options))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.SeedDatabase(data, 2019);     // Schuljahr; 2019 für 2019/20
}
```

## Starten des Programmes

In der Konsole wird mit folgendem Befehl das Skript gestartet. Je nach verwendeter Datenbankkonfiguration
wird die SQLite Datenbank in dieses Verzeichnis geschrieben.

```text
dotnet run
```

