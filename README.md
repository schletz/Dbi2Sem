# DBI im 2. Semester

## Lehrinhalte

### Bereich SQL

1. [Nicht korrespondierende Unterabfragen, die einen Wert liefern](11_SingleValueNonCorresponding)
2. [Korrespondierende Unterabfragen, die einen Wert liefern](12_SingleValueCorresponding)
3. [Unterabfragen, die Listen liefern (IN, NOT IN, EXISTS)](13_ListSubqueries)
4. [Unterabfragen, die in FROM verwendet werden](14_FromSubqueries)

### Bereich Datenbankadministration

1. [Views](21_Views)
2. [NULL Values](22_Null)
3. [Transaktionen](23_Transaktionen)

### Bereich ER Modellierung

1. Normalformen
2. Generalisierung
3. Hierarchien
4. Gruppen und Rollen
5. Modellierungsprojekt

## Die verwendete Schuldatenbank

- [Download als SQLite Datenbank](Schule.db)
- [Download als Access Datenbank](Schule.mdb)
- [Download als Sql Server (LocalDB) Datenbank](Schule.mdf)

Um die SQL Server Datenbank zu verwenden, müssen Sie die Datei im SQL Server Management Studio (SSMS)
mit folgender Abfrage einhängen:

```sql
USE [master]
GO
CREATE DATABASE [Schule] ON (FILENAME = N'C:\PATH\Schule.mdf') FOR ATTACH
GO
```

![](schuldb.png)


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
