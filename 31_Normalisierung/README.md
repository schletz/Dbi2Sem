# Normalisierung

## Funktionale Abhängigkeit

Bei den Normalformen kommt oft der Begriff *funktionale Abhängigkeit* vor. In der Mathematik ist
eine Funktion eine Vorschrift, die einen Eingangswert auf einen Ausgangswert abbildet. So bildet
die Funktion *f(x) = x + 1* jede Zahl auf die um 1 erhöhte Zahl ab. Dabei gilt, dass der gleiche Eingangswert immer auf den selben Ausgangswert abgebildet wird.

Nun kommen in Datenbanken keine mathematischen Formeln zum Einsatz, eine Funktion ist in diesem
Fall ein Nachsehen in der Datenbank. Es wird die Frage gestellt, ob ich durch einen Wert (meist der
Primärschlüssel) zusätzliche Informationen abrufen kann.

> Bei einer funktionalen Abhängigkeit gilt: Gleiche Eingangswerte bedeuten gleiche
> Ausgangswerte. Logisch ident ist auch die Aussage ungleiche Ausgangswerte haben
> ungleiche Eingangswerte.

Wir betrachten die folgende Tabelle:

| COL_0 (PK) | COL_1 | COL_2 | COL_3 | COL_4 |
| :--------: | :---: | :---: | :---: | :---: |
|     1      |   1   |   1   |   1   |   2   |
|     2      |   1   |   1   |   1   |   2   |
|     3      |   1   |   2   |   4   |   6   |
|     4      |   2   |   2   |   4   |   6   |
|     5      |   2   |   3   |   4   |   7   |
|     6      |   2   |   4   |   4   |   8   |

- **COL_0** ist ein Autowert, er kann daher von keiner Spalte abhängig sein.
- **COL_1** ist von keiner Spalte abhängig, denn es gibt keine Spalte, die immer dieselben Werte
  in *COL_1* hat wenn der Spaltenwert der jeweiligen Spalte ident ist.
- **COL_2** ist von keiner Spalte abhängig, es gilt ebenfalls die obige Aussage.
- **COL_3** ist von *COL_2* abhängig, denn es ist der Wert in *COL_3* gleich, wenn die Werte in
  *COL_2* gleich sind.
- **COL_4** ist von *COL_2* und *COL_3* abhängig (es ist die Summe dieser Zahlen).

## Unnormalisierte Daten

Unnormalisierte Daten können alle Daten in Tabellenform sein. Sie können mehrere Werte
pro Zelle (Beistrichliste) oder zusammengesetzte Werte (Vor- und Nachname in einer Zelle)
enthalten.

| Tag (PK) | Stunde (PK) | Klasse (PK) | Abteilung  | Fach | Fachnanme     | Lehrer   | Raum  |
| -------- | ----------- | ----------- | ---------- | ---- | ------------- | -------- | ----- |
| 2        | 3           | 5AHIF       | Informatik | DBI  | Datenbanken   | HIK, SZ  | C3.10 |
| 2        | 4           | 5AHIF       | Informatik | DBI  | Datenbanken   | HIK, SZ  | C3.10 |
| 3        | 3           | 5AHIF       | Informatik | POS  | Programmieren | SZ       | C4.06 |
| 3        | 4           | 5AHIF       | Informatik | POS  | Programmieren | SZ       | C4.06 |
| 4        | 7           | 5BHIF       | Informatik | DBI  | Datenbanken   | MOH, NIJ | C4.07 |
| 4        | 8           | 5BHIF       | Informatik | DBI  | Datenbanken   | MOH, NIJ | C4.07 |

## 1. Normalform

Die erste Normalform besagt, dass die Attribute einfache Wertausprägungen enthalten. Das bedeutet,
dass pro Zelle nur ein Wert stehen darf. In unserem Beispiel trifft das auf die Spalte *Lehrer*
nicht zu, denn sie umfasst mehrere Werte. Wir lösen das Problem so auf, indem wir eine
zusätzliche Zeile einfügen. Durch diese Zeilen müssen wir den Schlüssel auch anpassen, imdem wir
den Lehrer (der mehrfach vorkam) einfügen.

| Tag (PK) | Stunde (PK) | Klasse (PK) | Abteilung  | Fach | Fachnanme     | Lehrer (PK) | Raum  |
| -------- | ----------- | ----------- | ---------- | ---- | ------------- | ----------- | ----- |
| 2        | 3           | 5AHIF       | Informatik | DBI  | Datenbanken   | HIK         | C3.10 |
| 2        | 3           | 5AHIF       | Informatik | DBI  | Datenbanken   | SZ          | C3.10 |
| 2        | 4           | 5AHIF       | Informatik | DBI  | Datenbanken   | HIK         | C3.10 |
| 2        | 4           | 5AHIF       | Informatik | DBI  | Datenbanken   | SZ          | C3.10 |
| 3        | 3           | 5AHIF       | Informatik | POS  | Programmieren | SZ          | C4.06 |
| 3        | 4           | 5AHIF       | Informatik | POS  | Programmieren | SZ          | C4.06 |
| 4        | 7           | 5BHIF       | Informatik | DBI  | Datenbanken   | MOH         | C4.07 |
| 4        | 7           | 5BHIF       | Informatik | DBI  | Datenbanken   | NIJ         | C4.07 |
| 4        | 8           | 5BHIF       | Informatik | DBI  | Datenbanken   | MOH         | C4.07 |
| 4        | 8           | 5BHIF       | Informatik | DBI  | Datenbanken   | NIJ         | C4.07 |

## 2. Normalform

Die 2. Normalform liegt dann vor, wenn - zusätzlich zur 1. Normalform - alle Attribute, die nicht
Teil des Schlüssels sind, von diesem funktional abhängig sind. Der Schlüssel ist in seiner
Gesamtheit zu sehen, in diesem Fall ist er eine Kombination aus 4 Spalten. Die Spalte Abteilung
in diesem Beispiel ist nicht vom gesamten Schlüssel abhängig, denn schon durch die Klasse
kann auf die Abteilung geschlossen werden.

> Bei der 2. Normalform lagen wir alle Attribute, die nur von einem Teil des Schlüssels
> abhängig sind, aus.

### Tabelle Unterricht

| Tag (PK) | Stunde (PK) | Klasse (PK) | Lehrer (PK) | Raum  | Fach | Fachnanme     |
| -------- | ----------- | ----------- | ----------- | ----- | ---- | ------------- |
| 2        | 3           | 5AHIF       | HIK         | C3.10 | DBI  | Datenbanken   |
| 2        | 3           | 5AHIF       | SZ          | C3.10 | DBI  | Datenbanken   |
| 2        | 4           | 5AHIF       | HIK         | C3.10 | DBI  | Datenbanken   |
| 2        | 4           | 5AHIF       | SZ          | C3.10 | DBI  | Datenbanken   |
| 3        | 3           | 5AHIF       | SZ          | C4.06 | POS  | Programmieren |
| 3        | 4           | 5AHIF       | SZ          | C4.06 | POS  | Programmieren |
| 4        | 7           | 5BHIF       | MOH         | C4.07 | DBI  | Datenbanken   |
| 4        | 7           | 5BHIF       | NIJ         | C4.07 | DBI  | Datenbanken   |
| 4        | 8           | 5BHIF       | MOH         | C4.07 | DBI  | Datenbanken   |
| 4        | 8           | 5BHIF       | NIJ         | C4.07 | DBI  | Datenbanken   |

### Tabelle Klasse

| Nr (PK) | Abteilung  |
| ------- | ---------- |
| 5AHIF   | Informatik |
| 5BHIF   | Informatik |

## 3. Normalform

Bei der 3. Normalform werden *transitive Abhängigkeiten* beseitigt. Das bedeutet, dass
Spalten, die von Spalten außerhalb des Primärschlüssels abhängig sind, ausgelagert werden.
In diesem Beispiel ist der Fachname von der Kurzbezeichnung abhängig, und nicht vom
Schlüssel. Daher wird diese Information ausgelagert.

Die Trennung zwischen 2. und 3. Normalform ist oft schwer nachzuvollziehen, denn intuitiv
führt man im Normalisierungsschritt bereits beide Normalisierungen auf einmal aus.

> In der 3. Normalform werden Spalten, die von nicht Schlüsselspalten abhängen, ausgelagert.

### Tabelle Unterricht

| Tag (PK) | Stunde (PK) | Klasse (PK) | Lehrer (PK) | Raum  | Fach |
| -------- | ----------- | ----------- | ----------- | ----- | ---- |
| 2        | 3           | 5AHIF       | HIK         | C3.10 | DBI  |
| 2        | 3           | 5AHIF       | SZ          | C3.10 | DBI  |
| 2        | 4           | 5AHIF       | HIK         | C3.10 | DBI  |
| 2        | 4           | 5AHIF       | SZ          | C3.10 | DBI  |
| 3        | 3           | 5AHIF       | SZ          | C4.06 | POS  |
| 3        | 4           | 5AHIF       | SZ          | C4.06 | POS  |
| 4        | 7           | 5BHIF       | MOH         | C4.07 | DBI  |
| 4        | 7           | 5BHIF       | NIJ         | C4.07 | DBI  |
| 4        | 8           | 5BHIF       | MOH         | C4.07 | DBI  |
| 4        | 8           | 5BHIF       | NIJ         | C4.07 | DBI  |

### Tabelle Klasse

| Nr (PK) | Abteilung  |
| ------- | ---------- |
| 5AHIF   | Informatik |
| 5BHIF   | Informatik |

### Tabelle Fach (neu)

| Nr (PK) | Fachname      |
| ------- | ------------- |
| DBI     | Datenbanken   |
| POS     | Programmieren |

## Auswirkungen der Normalisierung

- Mehr Tabellen, daher auch mehr Joins bei Abfragen notwendig
- Tabellen haben weniger Attribute
- Keine Redundanz
- Updates, Insert und Delete Befehle können in der Regel schnell ausgeführt werden
- Weniger Speicher wird benötigt
- Weniger Anomalien (Änderungen können bei Beachtung der Primärschlüssel- und Fremdschlüsselbedingung keine Inkonsistenzen hervorrufen)
- Erhöhung der Integritätsbedingungen
- Für "große" Abfragen schlechtere Lese-Performance, Datenmodelle in Business Intelligence sind tw. denormalisiert
- Modell ist komplexer als ein denormalisiertes Modell

## Übung

**(1)** Führen Sie eine formale Normalisierung der ersten Tabelle aus dem Punkt funktionale
Abhängigkeit durch. Legen Sie dafür TabelleA, TabelleB, ... an.

**(2)** Fügen Sie in die Tabelle der 1. Normalform eine Spalte ID als Autowert ein. Wie sieht dann
die 2. und 3. Normalform aus? Welche Aussage kann über Autowerte und die 2. Normalform getroffen
werden? Tipp: Kopieren Sie die Tabelle im Microsoft Excel oder ein Tabellenverarbeitungsprogramm,
um die Normalisierung durchzuführen.

**(3)** Nachfolgende Tabelle entstand aus einer Abfrage über die Sprachreisen unserer Schule. Sie
liegt bereits in der 1. Normalform vor. Führen Sie die folgenden Aufgaben durch, indem sie die
Tabelle in ein Tabellenverarbeitungsprogramm kopieren. Verwenden Sie als Primärschlüssel die Spalten 
*LNr, Funktion, Klasse und Schuljahr*.

Führen Sie die Normalisierungsschritte zur 2. und 3. Normalform durch.

| LNr | Zuname    | Vorname   | Email                     | Funktion  | Ziel      | Von        | Bis        | AnzSchueler | Klasse | Schuljahr |
| --- | --------- | --------- | ------------------------- | --------- | --------- | ---------- | ---------- | ----------: | ------ | --------- |
| VVL | Vukovic   | Vladimir  | vukovic@spengergasse.at   | Begleiter | Malta     | 30.11.2018 | 07.12.2018 |          24 | 4BHIF  | 2018/19   |
| SUN | Subotic   | Nenad     | subotic@spengergasse.at   | Leiter    | Malta     | 30.11.2018 | 07.12.2018 |          24 | 4BHIF  | 2018/19   |
| FAV | Fakitsch  | Viktoria  | fakitsch@spengergasse.at  | Leiter    | Malta     | 30.11.2018 | 07.12.2018 |          25 | 4AHIF  | 2018/19   |
| KOP | Kock      | Philipp   | kock@spengergasse.at      | Begleiter | Edinburgh | 23.02.2019 | 04.03.2019 |          25 | 4EHIF  | 2018/19   |
| FAV | Fakitsch  | Viktoria  | fakitsch@spengergasse.at  | Leiter    | Edinburgh | 23.02.2019 | 04.03.2019 |          25 | 4EHIF  | 2018/19   |
| FAV | Fakitsch  | Viktoria  | fakitsch@spengergasse.at  | Begleiter | Cambridge | 26.02.2019 | 05.03.2019 |          25 | 4CHIF  | 2018/19   |
| NI  | Niemeczek | Claudia   | niemeczek@spengergasse.at | Leiter    | Cambridge | 26.02.2019 | 05.03.2019 |          25 | 4CHIF  | 2018/19   |
| NI  | Niemeczek | Claudia   | niemeczek@spengergasse.at | Begleiter | Edinburgh | 14.06.2018 | 21.06.2018 |          25 | 4AHIF  | 2017/18   |
| KEM | Keminger  | Alexander | keminger@spengergasse.at  | Leiter    | Edinburgh | 14.06.2018 | 21.06.2018 |          25 | 4AHIF  | 2017/18   |