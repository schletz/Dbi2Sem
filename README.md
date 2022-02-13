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
                <li><a href="14_FromSubqueries">Views und Unterabfragen in FROM</a></li>
            </ol>
        </td>    
        <td colspan="2" valign="top">
            <b>3</b> ER Modellierung
            <ol>
                <li><a href="31_Normalisierung">Normalformen</a></li>
                <li><a href="32_Generalisierung">Generalisierung</a></li>
                <li><a href="33_Historisierung">Historisierung</a></li>
                <li><a href="34_EntityAttribute">Entity Attribute Model</a></li>
                <li><a href="35_RekursiveBeziehungen">Rekursive Beziehungen</a></li>
            </ol>
        </td>
    </tr>
    <tr>
        <td colspan="2" valign="top">
            <b>2</b> Bereich Datenbankadministration
            <ol>
                <li><a href="01_OracleVM/03_Docker/README.md">Oracle als Docker Image</li>
                <li><a href="21_Null">Umgang mit NULL</a></li>
                <li><a href="22_Transactions">Transaktionen</a></li>
                <li><a href="23_Index">Verwenden von Indizes</a></li>
            </ol>
        </td>
        <td colspan="3" valign="top">
            <b>4</b> Modellierungsprojekt
            <ol>
                <li><a href="VorlageDbiModellierungsprojekt.docx">Wordvorlage</li>
            </ol>
        </td>
    </tr>
</table>

## Informationen zum Start

- [Download und Konfiguration der VM](01_OracleVM/README.md) **oder** [Oracle 21 XE als Docker Image](01_OracleVM/03_Docker/README.md)
- [Installation von DBeaver](01_OracleVM/01_Dbeaver/README.md)
- [Optional: Installation von JetBrains DataGrip](01_OracleVM/02_DataGrip/README.md)
- [Installation von erwin Data Modeler](02_ErWin/README.md)
- [Installation von PlantUML in VS Code](03_PlantUml/README.md)
- [Anlegen der Musterdatenbank (SchulDb)](SchulDbGenerator/README.md)

## Die verwendete Schuldatenbank

- [Download als SQLite Datenbank](Schule.db)
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
