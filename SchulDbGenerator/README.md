# SchulDb Generator

Dieses Programm generiert - basierend auf den realen Untisdaten - Musterdaten für Schüler und
Prüfungen. Es verwendet EF Core und schreibt die gesamte Datenbank neu.

## Voraussetzung: .NET 6

Dieses Programm verwendet .NET 6. Es kann daher unter Windows, Linux (64bit) und unter macOS
ausgeführt werden. Prüfen Sie vorher in der Eingabeaufforderung (Konsole), ob
Sie die entsprechende .NET Code Version installiert haben:

```text
dotnet --version
```

Liefert dieser Befehl einen Fehler oder ist die .NET Version kleiner als 6, laden Sie von der
[dotnet Download Seite](https://dotnet.microsoft.com/download) die neueste SDK Version
(nicht die Runtime) von .NET Core für Ihr Betriebssystem. Nach der Installation müssen Sie die
Eingabeaufforderung beenden und neu öffnen.

## Starten des Programmes

Nun kann die Datenbank mit dem Generator erzeugt werden:

```text
cd SchulDbGenerator
dotnet run -c Release
```

Das Programm fragt ab, welche Datenbank Sie anlegen möchten. Es kann eine SQLite Datei, eine SQL
Server Datenbank (LocalDb) oder eine Oracle Datenbank angelegt werden.

```text
Welche Datenbank soll erstellt werden? [1]: SQLite (Default)   [2]: LocalDb   [3]: Oracle 12 (VM)   [4]: Oracle 19 XE oder 21 XE  
```

- Um sich zur SQLite Datenbank zu verbinden, öffnen Sie DBeaver oder DataGrip und wählen eine SQLite
  Verbindung.
- Für den Zugriff auf die SQL Server Datenbank benötigen Sie das
  [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15).
  Dort können Sie sich zum Server *(LocalDb)\MSSQLLocalDB* (Windows Authentication) verbinden.
- Für die Verwendung der Oracle Datenbank muss die Virtuelle Maschine vor der Generierung natürlich
  gestartet werden. Nachdem die Datenbank erzeugt wurde, können Sie sich mit dem angezeigten
  Benutzerdaten verbinden.
