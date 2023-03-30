# Datenbankprojekt im Sommersemester

Es kann alleine oder zu zweit gearbeitet werden.
Alle Daten sind im Repository zu speichern.
Nach jeder DBI Stunde, an der am Projekt gearbeitet werden kann, ist ein Commit zu erstellen.
Die Commits werden zur Feststellung der Mitarbeit herangezogen.
Zur Ideenfindung oder als Basis der Modellierung kann GPT verwendet werden.
Das SQL Skript soll im Docker Container für SQL Server in der neuesten Version lauffähig sein:

```
docker run -d -p 1433:1433  --name sqlserver2019 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SqlServer2019" mcr.microsoft.com/azure-sql-edge
```

## Vorbereitung des Repositories

Stelle sicher, dass deine lokale git Installation wie auf https://github.com/schletz/Pos3xhif/blob/master/06%20Git/01_installation.md beschrieben konfiguriert ist.
Verwende das Repo aus dem Wintersemester.
Öffne danach die **git bash** (nicht den Windows Command) im Verzeichnis des Repositories und führe danach die folgenden Befehle aus.
*git reset* holt den Stand von github und löscht lokale Änderungen.
Sie schreiben ein *.gitignore*, welches Builds, Configs, etc. ignoriert.
Danach wird der Index mit `git rm` gelöscht.
Mit `git clean` werden alle Dateien, die wir ignorieren, gelöscht.

```bash
git fetch --all
git reset origin/main --hard
echo "**/bin" > .gitignore
echo "**/obj" >> .gitignore
echo "**/.vs" >> .gitignore
echo "**/.vscode" >> .gitignore
git add -A
git commit -m "Ready for 2nd term"

git rm -rf --cached .
git clean -dfX
git add -A
git commit --amend --no-edit
git push --force

```

### Erstellung eines README.md Dokumentes

Kopiere den Inhalt aus der Vorlage (unten) in die Datei *README.md*, passe sie entsprechend an und führe einen Commit in den main Branch durch.

### Erstellen der Feature Branches für die Beurteilungsstufen (Noten)

Öffne die *git bash* im Verzeichnis des Repositories und führe danach die folgenden Befehle aus.

```bash
for branch in grade-genuegend grade-befriedigend grade-gut grade-sehr-gut
do
    git checkout -b $branch
    git push -u origin $branch
done
```

Sichere nun den main Branch in github mittels *Settings - Branches - Branch protection rules* ab:

- Für *Branch name pattern* wird *main* verwendet.
- Als Regel wird *Require a pull request before merging* und *Require approvals* definiert.

Arbeite in den entsprechenden Feature Branches.
Bei der Fertigstellung eines Features führe den Merge in den main Branch über einen pull request durch.
**Der Lehrer** wird dann den Pull Request annehmen oder bei Bedarf mit Kommentaren zurückweisen.
Danach kannst du den nächsten Feature Branch mit `git pull origin main --rebase` auf den aktuellen Stand bringen.

Es gilt folgendes Bewertungsschema, *regelmäßige Commits vorausgesetzt*.

## Bewertung Genügend

Erstelle in PlantUML ein logisches ER Modell **in der Datei er_logical.puml**, welches folgende Themen beinhaltet:

- Ein Datenmodell, das der 3. Normalform genügt.
  Die Anzahl der Tabellen wird bewusst nicht vorgegeben, es soll die Problemstellung mit allen gelernten Prinzipien der Datenmodellierung abgedeckt werden.
  - Lookup Tabellen statt Strings z. B. für Kategorien, ...
  - Korrekte Wahl der Kardinalitäten. n : m soll schon im logischen Modell aufgelöst werden.
  - Korrekte naming conventions (Fremdschlüssel = Tabellenname + Attribut des PK in der anderen Tabelle)
  - Mehrteilige Schlüssel sollen in der logischen Version noch vorhanden sein.
  - Sinnvolle NULL bzw. NOT NULL Constraints.
  - Korrekte Anwendung der Notation für Beziehungen (identifizierend/nicht identifizierend, crowfeet)
- Eine (nicht aufgelöste) Generalisierung, vgl. https://github.com/schletz/Dbi2Sem/tree/master/32_Generalisierung.
  Die Generalisierung kann in PlantUML mit dem Vererbungspfeil gekennzeichnet werden.

Danach wird das physische ER Modell **in der Datei er_physical.puml** abgeleitet.
Es wird auch in PlantUML erstellt und beinhaltet folgende Informationen:

- SQL Server spezifische Datentypen, vgl. https://github.com/schletz/Dbi1Sem/blob/master/02_DDL/01_CreateTableCommand.md#create-table-statements
- Statt zusammengesetzten Schlüsseln wird z. B. eine Auto Increment ID erstellt.
- Sinnvolle UNIQUE Constraints, um zusammengesetzte Schlüssel im logischen Modell korrekt mit diesem Contraint zu versehen.
- Sinnvolle CHECK Constraints.
  
PlantUML Referenz: https://plantuml.com/en/ie-diagram

Am Ende wird in der Datei **create_db.sql** ein SQL Skript erstellt, welches das physische Modell für SQL Server abbildet.
Statt eines auto increment Wertes (*IDENTITY*) kann ein fixer INTEGER Wert verwendet werden.
Das erleichtert das Schreiben der *INSERT* Anweisungen.

**Beispiel für ein Entity mit Datentypen und Constraints**

<details>
<summary>Inhalt anzeigen</summary>

```text
@startuml
hide circle
entity Schoolclass {
    * Id : INTEGER <<generated>>
    ---
    * Shortname : VARCHAR(16)
    * Schoolyear : INTEGER
    * DepartmentId : INTEGER <<FK>> 
    -- constraints --
    UNIQUE (Shortname, Schoolyear)
    CHECK (Schoolyear > 2000)
}
@enduml
```
</details>

**Vorlage für das Create SQL Skript create_db.sql (SQL Server)**

<details>
<summary>Inhalt anzeigen</summary>

Hinweis: Tausche *\<your_dbname\>* 2x aus (einmal in *DECLARE* und dann bei *USE*).

```sql
USE tempdb;
GO    
BEGIN
    DECLARE @DBNAME AS VARCHAR(MAX) = '<your_dbname>'
    IF EXISTS(SELECT * FROM sys.databases WHERE Name = @DBNAME)
    BEGIN
        -- Disconnect all users and recreate database.
        EXEC('ALTER DATABASE ' + @DBNAME + ' SET SINGLE_USER WITH ROLLBACK IMMEDIATE');
        EXEC('DROP DATABASE ' + @DBNAME);
    END;
    EXEC('CREATE DATABASE ' + @DBNAME);
END;
USE <your_dbname>;   -- Change to your database name (USE does not allow variables)
GO

-- Write your CREATE TABLE Statements
```
</details>

## Bewertung Befriedigend

Es wird im physischem ER Modell **einer** der nachfolgenden Punkte ergänzt: 

1. Ergänzung des Modelles um ein Entity – Attribute – Value Modell (siehe https://github.com/schletz/Dbi2Sem/tree/master/34_EntityAttribute).
   Es kann wahlweise mit Entities modelliert oder als JSON Spalte angegeben werden.
   Bei einer JSON Spalte ist ein JSON Schema (https://json-schema.org/learn/getting-started-step-by-step#defining-the-properties) zu definieren.

2. Ergänzung des Modelles um eine rekursive Beziehung (siehe https://github.com/schletz/Dbi2Sem/tree/master/35_RekursiveBeziehungen).

Das SQL Skript *create_db.sql* muss natürlich auch angepasst werden, um das neue Modell zu generieren.
Das logische Modell muss nicht angepasst werden.

## Bewertung Gut

Es werden im physischen ER Modell **beide** Punkte aus Befriedigend implementiert.

Zusätzlich wird eine *VIEW* im SQL Skript definiert, die einen der beiden Punkte auf eine flache Tabelle abbildet.
Das ist bei einem Entity – Attribute – Value Modell eine Tabelle mit den Attributen als Spalte. Hier kann mit PIVOT gearbeitet werden.
Bei einer rekursiven Beziehung können dies Spalten mit dem Parent Datensatz sein (z. B. Chef des Mitarbeiters).
Die Rekursionstiefe ist auf eine sinnvolle Anzahl zu begrenzen.
Rekursives SQL ist zwar möglich, soll aus Performancegründen aber vermieden werden.

Im SQL Skript *create_db.sql* sollen vor der Definition der VIEW einige Musterdaten mit INSERT geschrieben werden, um die Funktionalität zu testen.

## Bewertung Sehr gut

Schreibe ein Programm, das auf diese VIEW mit dem OR Mapper EF Core zugreift.
Die Einbindung von Views wird auf https://github.com/schletz/Dbi4Sem/blob/master/11_EFCoreAccess/README.md#erstellen-und-nutzen-einer-view erklärt.
Das Programm soll folgende Schritte durchführen:

- Beim Programmstart sollen die Tabellen, auf die die VIEW zugreift, geseeded werden (z. B. mit Bogus).
- Daher sind nur die benötigten Tabellen als Modelklasse abzubilden.
  Wenn diese Tabellen allerdings andere Tabellen als FK referenzieren, brauchst du ebenfalls eine Modelklasse hierfür.
- Nach dem Seeden wird die VIEW ausgelesen und die Daten werden angezeigt.
- Wenn du JSON Spalten verwendest, kannst du mit *System.Text.Json.JsonSerializer* ein Objekt als JSON String einfügen.

## Anhang: Vorlage für die Datei README.md

````
# Datenbankprojekt im Sommersemester 2022/23

## Team

| Klasse | Name             | E-Mail                  |
| ------ | ---------------- | ----------------------- |
| 3xHIF  | Max *Mustermann* | mus1234@spengergasse.at |
| 3xHIF  | Max *Mustermann* | mus1234@spengergasse.at |


## Thema des Datenmodelles

(Beschreibe mit 2-3 Absätzen, welches Thema du modellierst.)

## ER Modell (gerendert)

### Logisches Modell

```
Kopiere den Sourcecode auf https://www.plantuml.com/plantuml und rufe über den SVG Link die URL der SVG Grafik ab.
Bette danach das Bild mit ![](<URL>) ein.
```

### Physisches Modell

```
Kopiere den Sourcecode auf https://www.plantuml.com/plantuml und rufe über den SVG Link die URL der SVG Grafik ab.
Bette danach das Bild mit ![](<URL>) ein.
```

### Rohdateien

- [Logisches ER Modell](er_logical.puml)
- [Physisches ER Modell](er_physical.puml)
- [SQL Skript](create_db.sql)
````