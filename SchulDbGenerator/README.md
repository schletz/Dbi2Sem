# SchulDb Generator

Dieses Programm generiert - basierend auf den realen Untisdaten - Musterdaten für Schüler und
Prüfungen. Es verwendet EF Core und schreibt die gesamte Datenbank neu.

## Generieren der Datenbank in einem Oracle oder SQL Server Container

Öffne in Docker Desktop eine Shell des Oracle oder SQL Server Containers. Kopiere danach die
folgenden Befehle in das Fenster. Sie laden die .NET 6 SDK und den Generator der Datenbank.
Am Ende wirst du nach dem Admin Passwort der Datenbank gefragt. Hast du den Container mit den
Standardpasswörtern (*oracle* für Oracle bzw. *SqlServer2019* für Sql Server 2019) erstellt,
musst du nur *Enter* drücken.

```bash
if [ -d "/opt/oracle" ]; then DOWNLOADER="curl -s"; else DOWNLOADER="wget -q -O /dev/stdout"; fi
$DOWNLOADER https://raw.githubusercontent.com/schletz/Dbi2Sem/master/start_dotnet.sh > /tmp/start_dotnet.sh
chmod a+x /tmp/start_dotnet.sh
/tmp/start_dotnet.sh https://raw.githubusercontent.com/schletz/Dbi2Sem/master/SchulDbGenerator/SchulDbGenerator.tar

```

Das Programm fragt ab, welche Datenbank Sie anlegen möchten. Wählen Sie **(4)**, um die
Datenbank in einem Oracle Container anzulegen oder **(5)**, um die Datenbank in einem SQL Server
Container anzulegen.

```text
Welche Datenbank soll erstellt werden? [1]: SQLite (Default)   [2]: LocalDb   [3]: Oracle 12 (VM)   [4]: Oracle 19 XE oder 21 XE   [5] SQL Server Docker Image
```

## Alternative: Generieren der Datenbank im Host Betriebssystem

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

Nun kann die Datenbank mit dem Generator erzeugt werden:

```text
cd SchulDbGenerator
dotnet run -c Release
```

Das Programm fragt ab, welche Datenbank Sie anlegen möchten. Es kann eine SQLite Datei, eine SQL
Server Datenbank (LocalDb oder Docker Image) oder eine Oracle Datenbank angelegt werden.

```text
Welche Datenbank soll erstellt werden? [1]: SQLite (Default)   [2]: LocalDb   [3]: Oracle 12 (VM)   [4]: Oracle 19 XE oder 21 XE   [5] SQL Server Docker Image
```

