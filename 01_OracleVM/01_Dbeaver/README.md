# Arbeiten mit DBeaver Community

Als SQL Editor kann auch DBeaver verwendet werden. Er kann sich über die JDBC Treiberarchitektur
zu verschiedenen Datenbanken - darunter auch Oracle - verbinden. Das Programm kann auf der [DBeaver Downloadseite](https://dbeaver.io/download/)
heruntergeladen werden.

## Voraussetzung

Das Portforwarding des Port 1521 muss - wie im Kapitel Installation beschrieben - in Virtual Box
gesetzt werden.

## Verbinden zur Oracle Datenbank in der VM (Oracle 11 oder 12)

Durch den Button *New Database Connection* kann der Verbindungsdialog geöffnet werden. In diesem Dialog
muss Oracle als Datenbanksystem ausgewählt werden:

![](dbeaver01.png)

Zum Verbinden müssen wie in SQL Developer die Verbindungsdaten eingegeben werden:
- Host: *localhost*
- Database: *orcl* (Service Name)
- Username: *SchulDb* (oder *System*, falls noch kein User existiert)
- Passwort: *oracle*

Beim ersten Verbinden wird der Treiber aus dem Netz geladen.

## Verbinden zur Oracle Datenbank in der VM (Oracle 19 XE oder 21 XE)

Oracle 19 oder 21 arbeiten mit pluggable databases. Daher ist die Verbindungsinformation anders:

![](dbeaver01.png)

Zum Verbinden müssen wie in SQL Developer die Verbindungsdaten eingegeben werden:
- Host: *localhost*
- Database: *XEPDB1* (Service Name)
- Username: *SchulDb* (oder *System*, falls noch kein User existiert)
- Passwort: *oracle*

Beim ersten Verbinden wird der Treiber aus dem Netz geladen. Die angebotenen pluggable databases
können herausgefunden werden, indem man sich mit dem User *system* und dem Service Name *XE*
verbindet. Danach wird das SQL Statement `SELECT name FROM v$pdbs;` abgesetzt.

## Zugriff auf das Schema

DBeaver listet alle Schemata der Datenbank auf. Um SQL Abfragen im Schema *SchulDB*
auszuführen, muss ein SQL Editor geöffnet werden. Am Schnellsten geht das mit *STRG* + *ENTER*, es gibt
auch einen Button in der Symbolleiste. Achten Sie darauf, dass das richtige Schema in der Symbolleiste
als Ziel der Abfrage ausgewählt ist. Sonst bekommen Sie den Fehler *table or view does not exist*.

EInzelne Abfragen werden mit *STRG* + *ENTER* ausgeführt. Möchten Sie das ganze SQL Skript ausführen, so
klicken Sie auf *Execute Script* oder drücken *ALT* + *X*.

![](dbeaver03.png)

## Deaktivieren der Tabellen Aliasnamen

Die Autocomplete Funktion weist Tabellen in der FROM Klausel einen einstelligen Alias zu. Da Oracle jedoch
die Syntax mit AS im FROM nicht versteht, muss dieses Feature unter *Window* - *Preferences* deaktiviert werden:

![](dbeaver5.png)

## Diagramme erzeugen

Ein nettes Feature ist das automatische Erzeugen von ER Diagrammen von einem Schema aus. Dies erreicht
man im Kontextmenü des Schemas in der Navigation:

![](dbeaver04a.png)

## Zugriff auf Access Datenbanken

Sie können auch eine neue Verbindung zu einer Access Datenbank herstellen. Dabei wird beim erstmaligen
Verbinden der JDBC Treiber geladen.

