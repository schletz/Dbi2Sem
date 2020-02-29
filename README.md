# DBI im 2. Semester

## Lehrinhalte

<table>
    <tr>
        <th>1</th> <th>2</th> <th>3</th> <th>4</th> <th>5</th>
    </tr>
    <tr>
        <td colspan="3" valign="top">
            <b>1</b> Erweitertes SQL
            <ol>
                <li><a href="11_SingleValueNonCorresponding">Nicht korrelierte Unterabfragen, die einen Wert liefern</a></li>
                <li><a href="12_SingleValueCorresponding">Korrelierte Unterabfragen, die einen Wert liefern</a></li>
                <li><a href="13_ListSubqueries">Unterabfragen, die Listen liefern (IN, NOT IN, EXISTS)</a></li>
                <li>><a href="14_FromSubqueries">Views und Unterabfragen in FROM</a></li>
                <li>CASE, COALESCE, Datumswerte und andere Funktionen</li>
                <li>Window Functions</li>
            </ol>
        </td>    
        <td colspan="2" valign="top">
            <b>3</b> ER Modellierung
            <ol>
                <li><a href="31_Normalisierung">Normalformen</a></li>
                <li><a href="32_Generalisierung">Generalisierung</a></li>
                <li><a href="33_Historisierung">Historisierung</a></li>
                <li>Hierarchien</li>
                <li>Gruppen und Rollen</li>
            </ol>
        </td>
    </tr>
    <tr>
        <td colspan="2" valign="top">
            <b>2</b> Bereich Datenbankadministration
            <ol>
                <li>Arbeiten mit der Oracle VM</li>
                <li><a href="22_Null">NULL Values</a></li>
                <li><a href="23_Transactions">Transaktionen</a></li>
            </ol>
        </td>
        <td colspan="3" valign="top">
            <b>4</b> Modellierungsprojekt
        </td>
    </tr>
</table>

### Beurteilung

- Insgesamt 2 praktische Überprüfungen im Bereich SQL und Datenbankadministration.
- Abgabe des Modellierungsprojektes.
- Die Gewichtung erfolgt nach Stundenausmaß, wobei beide Teile (Modellierung und SQL/DB Admin)
  positiv sein müssen.

## Die verwendete Schuldatenbank

- [Download als SQLite Datenbank](Schule.db)
- [Download als Access Datenbank](Schule.mdb)
- [Download als Sql Server (LocalDB) Datenbank](Schule.mdf)

Um die SQL Server Datenbank zu verwenden, müssen Sie sich im SQL Server Management Studio (SSMS)
zur LocalDB mit dem Servernamen *(LocalDb)\MSSQLLocalDB* (Windows Authentifizierung) verbinden.
Danach hängen Sie die Datei mit folgender Abfrage ein:

```sql
USE [master]
GO
CREATE DATABASE [Schule] ON (FILENAME = N'C:\PATH\Schule.mdf') FOR ATTACH
GO
```

![](schuldb20200209.png)

## Synchronisieren des Repositories in einen Ordner

1. Laden Sie von https://git-scm.com/downloads die Git Tools (Button *Download 2.xx for Windows*)
    herunter. Es können alle Standardeinstellungen belassen werden, bei *Adjusting your PATH environment*
    muss aber der mittlere Punkt (*Git from the command line [...]*) ausgewählt sein.
2. Legen Sie einen Ordner auf der Festplatte an, wo die Daten gespeichert werden sollen
    (z. B. *C:\Schule\DBI\Repo*).
3. Initialisieren Sie den Ordner mit folgenden Befehlen, die in der Windows Konsole in diesem Verzeichnis
    ausgeführt werden:

```text
git init
git remote add origin https://github.com/schletz/Dbi2Sem.git
git fetch --all
git reset --hard origin/master
```

### Nachträgliches Synchronisieren

Führen Sie die Datei *resetGit.cmd* aus. Dadurch werden die lokalen Änderungen zurückgesetzt und der
neue Stand wird vom Server übertragen.
