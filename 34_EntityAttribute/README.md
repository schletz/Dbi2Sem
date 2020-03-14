# Entity – Attribute – Value Modell

[Folien als PDF](Entity_Attribute_Model.pdf) (Dank an Koll. Hilbe für die Bereitstellung!)

## Erklärung zu den Folien

**Seite 2:** Oft haben Entities Attribute, die nicht zu 100% bei der Modellierung feststehen. So kann
sich herausstellen, dass unser Auto zusätzliche Attribute bekommen soll (z. B. km Stand). Dies kann
nur durch Änderung des Schemas abgebildet werden, was im laufenden Betrieb vermieden werden soll.
Außerdem werden die Inhalte als Strings gespeichert, was eine Abfrage schwierig macht. So gibt
jemand für das Navi die Bezeichnung "TomTom 1" ein, ein anderer Datensatz hat den Wert "Tom Tom 1"
(mit Leerstelle). Somit ist ein Filtern schwierig.

**Seite 3:** Damit die Werte kategorisiert werden können, werden hier Lookup Tabellen verwendet. So
ist genau festgelegt, welche Radioarten eingetragen werden dürfen. Neue Merkmale können hier allerdings
auch nicht ohne Änderung des Schemas hinzugefügt werden.

**Seite 4:** Dieses Modell geht einen anderen Weg: Es sagt einfach "Ein Auto hat mehrere Eigenschaften".
Eine Eigenschaft ist das verbaute Navi, der km Stand, ... Die gespeicherten Eigenschaften können einfach
mit einem INSERT in die Tabelle Eigenschaft erweitert werden. Somit wird die Eigenschaft nicht als
Spalte, sondern als Zeile gespeichert.

**Seite 5:** In diesem Modell werden jedoch alle Eigenschaften gleich behandelt. Allerdings ist der
km Stand eine Zahl, das verbaute Navi ein String. Wollen wir Autos mit mehr als 100.000 km filtern,
müssen wir den Wert konvertieren (Typecast).

**Seite 6:** Aus diesem Grund unterscheiden wir in 2 Spalten Zahlen- und Textwerte. So ist beim km
Stand nur der Zahlenwert befüllt, der Zeichenwert ist NULL. Beim Navi ist es umgekehrt. Die Tabelle
Einheit speichert noch zusätzlich, dass es sich bei der Zahl des km Standes (z. B. 89.000) um die
Einheit "km" handelt. Beim km Stand kann man dies wohl auch ohne Einheit erkennen, bei der Leistung
wäre dies schon schwieriger (PS oder kW).

**Seite 8:** Bei Zeichenwerten haben wir aber wieder das Problem, dass sie frei eingegeben werden
können (siehe Seite 2). Also definieren wir sogenannte *Ausprägungen*, also Dropdownlisten in einem
Programm, wo nur diese Werte verwendet werden dürfen.

## Übungsangabe: Der Autokatalog

Die Firma WESLA überlegt Ihre Autos nur noch online zu verkaufen. Als ersten Schritt soll ein Datenmodell erstellt werden, in welcher alle möglichen Eigenschaften gespeichert werden können

Für jedes Auto soll auf jeden Fall die Fahrgestellnummer gespeichert, der Preis und die Treibstoffart. Diese Informationen stehen bei jedem Auto zur Verfügung. Zusätzlich sollen verschiedenste Eigenschaften des Autos festgehalten werden, bspw. Lichtanlage, Radioart, Fahrassistent, Multimedia System, Gewicht, Beschleunigung von 0 auf 100, Höchstgeschwindigkeit, durchschnittlicher Verbrauch, Tankvolumen und so weiter.

Wichtig sind dem Auftraggeber zwei Dinge. Die Liste der Eigenschaften soll erweiterbar sein und er legt hohen Wert auf gute Datenqualität. Stellen Sie daher sicher, dass soweit als möglich nur sinnvolle Werte eingetragen werden können und bei Zahlen mathematische Operationen angewendet werden können, wie beispielsweise durchschnittlicher Verbrauch (daher keine Buchstaben bei Gewicht, Tankvolumen oder durchschnittlicher Verbrauch!). Gehen Sie weiters davon aus, dass es sehr viele Eigenschaften gibt, jedoch in der Regel für jedes Auto nur sehr wenige Eigenschaften befüllt sind. Unten angeführt sind noch exemplarisch die derzeit möglichen Werte für die Eigenschaften.

- Lichtanlage: LED, Xenon, Normal
- Radio: USB, USB CD
- Fahrassistent: Parkhilfe, Parkhilfe und Abstand, Parkhilfe und Abstand und Spurassistent
- Multimedia System: TV Funktion, TV und Entertainment Funktion

Bei den Eigenschaften Gewicht, Beschleunigung von 0 auf 100, Höchstgeschwindigkeit, durchschnittlicher Verbrauch, Tankvolumen sollen nur Zahlenwerte eingetragen werden. Idealerweise speichern sie zu jedem Wert auch die Einheit ab.

## Zusatzinfo: Pivot in SQL Server

Zur Demonstration wird in SQL Server eine kleine Datenbank mit
Sensorwerten angelegt. Die Sensoren liefern Messwerte für Temperatur, Feuchtigkeit und Luftdruck.
Sensor 1 liefert alle Werte, während Sensor 2 nur die Temperatur messen kann.

Diese Beispiele können mit dem SQL Server Management Studio (SSMS) nachvollzogen werden. Beim
Verbinden kann die LocalDB unter dem Namen *(LocalDb)\MSSQLLocalDB* (Windows Authentifizierung)
verwendet werden.

```sql
USE [master]
DROP DATABASE [Sensor]
CREATE DATABASE Sensor;
USE Sensor

CREATE TABLE Sensor (
    SensorId INTEGER PRIMARY KEY
);

CREATE TABLE Valuename (
    ValuenameId INTEGER PRIMARY KEY,
    Name VARCHAR(128) NOT NULL,
);

CREATE TABLE Measurement (
    Date      DATETIME,
    Sensor    INTEGER REFERENCES Sensor(SensorId),
    Valuename INTEGER REFERENCES Valuename(ValuenameId),
    Value     DECIMAL(9,4) NOT NULL,
    PRIMARY KEY (Date, Sensor, Valuename)
);

INSERT INTO Sensor VALUES (1);
INSERT INTO Sensor VALUES (2);
INSERT INTO Valuename VALUES (1, 'Temperature');
INSERT INTO Valuename VALUES (2, 'Humidity');
INSERT INTO Valuename VALUES (3, 'Pressure');
-- Sensor 1 hat alle 3 Parameter (Temp, Feuchte und Druck)
INSERT INTO Measurement VALUES ('2020-03-10T00:00:00Z', 1, 1, 20.0);
INSERT INTO Measurement VALUES ('2020-03-10T01:00:00Z', 1, 1, 21.0);
INSERT INTO Measurement VALUES ('2020-03-10T02:00:00Z', 1, 1, 20.5);
INSERT INTO Measurement VALUES ('2020-03-10T03:00:00Z', 1, 1, 19.8);
INSERT INTO Measurement VALUES ('2020-03-10T00:00:00Z', 1, 2, 59);
INSERT INTO Measurement VALUES ('2020-03-10T01:00:00Z', 1, 2, 57);
INSERT INTO Measurement VALUES ('2020-03-10T02:00:00Z', 1, 2, 58);
INSERT INTO Measurement VALUES ('2020-03-10T03:00:00Z', 1, 2, 69);
INSERT INTO Measurement VALUES ('2020-03-10T00:00:00Z', 1, 3, 987);
INSERT INTO Measurement VALUES ('2020-03-10T01:00:00Z', 1, 3, 989);
INSERT INTO Measurement VALUES ('2020-03-10T02:00:00Z', 1, 3, 992);
INSERT INTO Measurement VALUES ('2020-03-10T03:00:00Z', 1, 3, 990);
-- Sensor 2 hat nur Temp
INSERT INTO Measurement VALUES ('2020-03-10T00:00:00Z', 2, 1, 25.0);
INSERT INTO Measurement VALUES ('2020-03-10T01:00:00Z', 2, 1, 26.0);
INSERT INTO Measurement VALUES ('2020-03-10T02:00:00Z', 2, 1, 25.5);
INSERT INTO Measurement VALUES ('2020-03-10T03:00:00Z', 2, 1, 24.8);
```

Nun wollen wir die durchschnittlichen Werte der Temperatur, der Feuchte und des Druckes abfragen.
Klassisch funktioniert dies mit Unterabfragen:

```sql
SELECT m.Date,
    (SELECT AVG(m2.Value) FROM Measurement m2 WHERE m2.Valuename = 1 AND m2.Date = m.Date) AS Temperature,
    (SELECT AVG(m2.Value) FROM Measurement m2 WHERE m2.Valuename = 2 AND m2.Date = m.Date) AS Humidity,
    (SELECT AVG(m2.Value) FROM Measurement m2 WHERE m2.Valuename = 3 AND m2.Date = m.Date) AS Pressure
FROM Measurement m
GROUP BY m.Date;     -- Jedes Datum nur 1x ausgeben.
```

SQL Server bietet mit dem Schlüsselwort *PIVOT* eine Alternative an:

```sql
-- Durchschnittliche Temperatur, Feuchte bzw. Druck
SELECT Date, [1] AS Temperature, [2] AS Humidity, [3] AS Pressure
FROM (SELECT Valuename, Value, Date FROM Measurement) AS SourceTable
PIVOT (AVG(Value) FOR Valuename IN ([1], [2], [3])) AS PivotTable;

-- Pivotierte Tabelle mit Temperatur, Feuchte bzw. Druck
SELECT Date, Sensor, [1] AS Temperature, [2] AS Humidity, [3] AS Pressure
FROM (SELECT Valuename, Value, Date, Sensor FROM Measurement) AS SourceTable
PIVOT (MIN(Value) FOR Valuename IN ([1], [2], [3])) AS PivotTable;
```

Die Abfrage mit *PIVOT* benötigt lt. Ausführungsplan nur 1/4 der Zeit, da hier die spezialisierte *PIVOT*
Funktion verwendet wurde. Die Spaltenwerte, nach denen pivotiert wird, müssen fix angegeben
werden. Dynamische Werte sind nur mit dynamisch generiertem SQL möglich, wobei die Auswertung des
Ergebnisses mit dynamischen Spalten in den Programmiersprachen auch schwieriger ist.

Weitere Informationen: https://docs.microsoft.com/en-us/sql/t-sql/queries/from-using-pivot-and-unpivot?view=sql-server-ver15

## Speichern von JSON Daten (SQL Server)

In SQL Server ab Version 2016 können auch JSON Daten verarbeitet werden. Sie werden zunächst einmal
als normale Strings (*VARCHAR*) in der Tabelle gespeichert. Danach werden Sie mit den entsprechenden
JSON Funktionen von SQL Server bearbeitet.

```sql
USE [master]
DROP DATABASE [Sensor]
CREATE DATABASE Sensor;
USE Sensor

CREATE TABLE Sensor (
    SensorId INTEGER PRIMARY KEY
);

CREATE TABLE Measurement (
    Date      DATETIME,
    Sensor    INTEGER REFERENCES Sensor(SensorId),
    Value     VARCHAR(MAX) NOT NULL,    -- Speichert die JSON Werte
    PRIMARY KEY (Date, Sensor)
);

INSERT INTO Sensor VALUES (1);
INSERT INTO Sensor VALUES (2);
-- Sensor 1 hat alle 3 Parameter (Temp, Feuchte und Druck)
INSERT INTO Measurement VALUES ('2020-03-10T00:00:00Z', 1, '{"temp": 20.0, "hum": 59, "pres": 987}');
INSERT INTO Measurement VALUES ('2020-03-10T01:00:00Z', 1, '{"temp": 21.0, "hum": 57, "pres": 989}');
INSERT INTO Measurement VALUES ('2020-03-10T02:00:00Z', 1, '{"temp": 20.5, "hum": 58, "pres": 992}');
INSERT INTO Measurement VALUES ('2020-03-10T03:00:00Z', 1, '{"temp": 19.8, "hum": 69, "pres": 990}');
-- Sensor 2 hat nur Temp
INSERT INTO Measurement VALUES ('2020-03-10T00:00:00Z', 2, '{"temp": 25.0}');
INSERT INTO Measurement VALUES ('2020-03-10T01:00:00Z', 2, '{"temp": 26.0}');
INSERT INTO Measurement VALUES ('2020-03-10T02:00:00Z', 2, '{"temp": 25.5}');
INSERT INTO Measurement VALUES ('2020-03-10T03:00:00Z', 2, '{"temp": 24.8}');

-- Ausgeben der einzelnen Teile als Tabelle
SELECT
    m.Date, m.Sensor,
    JSON_VALUE(m.Value, '$.temp') AS Temperature,
    JSON_VALUE(m.Value, '$.hum')  AS Humidity,
    JSON_VALUE(m.Value, '$.pres') AS Pressure
FROM Measurement m;

-- Flexibler mit OPENJSON (einer table value function in SQL Server)
SELECT *
FROM Measurement m CROSS APPLY OPENJSON (m.Value)
WITH (
    Temperature DECIMAL(9,4) '$.temp',
    Humidity    DECIMAL(9,4) '$.hum',
    Pressure    DECIMAL(9,4) '$.pres'
);
```

Weitere Informationen auf https://docs.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver15
