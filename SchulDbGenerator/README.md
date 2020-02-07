# SchulDb Generator

Dieses Programm generiert - basierend auf den realen Untisdaten - Musterdaten für Schüler und
Prüfungen. Es verwendet EF Core und schreibt die gesamte Datenbank neu.

## Kopieren der Untisdaten

Für das Untistool werden täglich auf `\\enterprise\administration\schmidsUntisTool` csv Daten aus
der Untisdatenbank exportiert. Kopiere die folgenden Dateien in das Verzeichnis `Untis/Data`
dieser Applikation. Der String *2019-2020* kann im Programm noch angepasst werden.

- *AlleLehrer/Lehrer_2019-2020.csv*
- *DataFiles_2019-2020/\*.csv*

## Starten des Programmes

In der Konsole kann das Programm - wenn die .NET Core SDK ab Version 3.1 vorhanden ist - kompiliert
und ausgeführt werden:

```text
cd SchulDbGenerator
dotnet run -c Release
```

Es wird der Typ der zu erstellenden Datenbank und weitere Informationen abgefragt.

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
