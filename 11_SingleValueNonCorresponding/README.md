# Nicht korrelierende Unterabfragen, die einen Wert liefern

Wir möchten als erstes Beispiel alle Schüler und das Geburtsdatum des jüngsten Schülers ausgeben.
Dies klingt einmal einfach, stellt uns aber vor ein Problem.

Einerseits erfolgt die Ausgabe der Schüler mit folgendem normalen SQL Statement:

```sql
SELECT s.S_Nr, s.S_Zuname, s.S_Vorname
FROM Schueler s;
```

Das Ermitteln des jüngsten Schülers ist auch keine schwierige Sache, es ist eine einfache *MAX()*
Funktion.

```sql
SELECT MAX(s.S_Gebdatum) FROM Schueler s;
```

Die Schwierigkeit ist die Kombination der beiden Ergebnisse. Denn wenn wir *MAX()* verwenden,
gruppieren wir die Daten. Es entsteht 1 Datensatz pro Tabelle. Andererseits möchten wir aber alle
Schülerdaten ausgeben.

Die Lösung bieten Unterabfragen. Sie können - je nach ihrem Rückgabewert - in 3 Kategorien
eingeteilt werden:

- Unterabfragen, die genau einen Wert liefern.
- Unterabfragen, die eine Liste von Werten (1 Spalte, mehrere Zeilen) liefern.
- Unterabfragen, die ganze Tabellen liefern.

Die Verwendung von Unterabfragen, die genau einen Wert liefern, ist sehr leicht. Sie werden dort
eingesetzt, wo normalerweise Spalten stehen. Unser Beispiel sieht dann so aus:

```sql
SELECT
    s.S_Nr, s.S_Zuname, s.S_Vorname,
    (SELECT MAX(s.S_Gebdatum) FROM Schueler s) AS Juengster
FROM Schueler s;
```

| S_Nr | S_Zuname   | S_Vorname | Juengster  |
| ---- | ---------- | --------- | ---------- |
| 1000 | Cartwright | Jaime     | 2005-08-31 |
| 1001 | Bogan      | Stanley   | 2005-08-31 |
| 1002 | Mertz      | Andy      | 2005-08-31 |
| ...  | ...        | ...       | ...        |

> **Hinweis:** Die Unterabfrage muss immer eingeklammert sein!

## Vergleich mit Programmcode

Hilfreich ist auch diese Analogie in den Programmiersprachen. Da die Unterabfrage keinerlei Daten
von der äußeren Abfrage braucht, wird sie sozusagen vorher ausgeführt und wie als Variable gespeichert.

```c#
var value = MySubQuery();
foreach (Abteilung a in db.Abteilungen)
{
    Console.WriteLine($"{a.AbtNr} {a.AbtLeiter} {value}")
}
```

## Verwendung mehrerer Unterabfragen

Natürlich können auch mehrere Unterabfragen verwendet werden. Somit kann z. B. das Geburtsdatum
des ältesten und jüngsten Schülers ermittelt werden:

```sql
SELECT
    s.S_Nr, s.S_Zuname, s.S_Vorname,
    (SELECT MAX(s.S_Gebdatum) FROM Schueler s) AS Juengster,
    (SELECT MIN(s.S_Gebdatum) FROM Schueler s) AS Aelterster
FROM Schueler s;
```

| S_Nr | S_Zuname   | S_Vorname | Juengster  | Aeltester  |
| ---- | ---------- | --------- | ---------- | ---------- |
| 1000 | Cartwright | Jaime     | 2005-08-31 | 1973-10-30 |
| 1001 | Bogan      | Stanley   | 2005-08-31 | 1973-10-30 |
| 1002 | Mertz      | Andy      | 2005-08-31 | 1973-10-30 |
| ...  | ...        | ...       | ...        | ...        |

## Unterabfragen in Ausdrücken

Da die angegebenen Unterabfragen nur 1 Wert zurückliefern, können sie auch in Ausdrücken verwendet
werden. Hier wird die Altersdifferenz in Tagen zum ältesten Schüler berechnet. Die Funktion *JULIANDAY()*
ist speziell für SQLite, denn sonst würde die Differenz in Jahren berechnet werden.

**SQLite**
```sql
SELECT
    s.S_Nr, s.S_Zuname, s.S_Vorname,
    JULIANDAY(s.S_Gebdatum) - JULIANDAY((SELECT MIN(s.S_Gebdatum) FROM Schueler s)) AS DiffZuAeltester
FROM Schueler s;
```

**Oracle**
```sql
SELECT
    s.S_Nr, s.S_Zuname, s.S_Vorname,
    EXTRACT(DAY FROM (s.S_Gebdatum - (SELECT MIN(s.S_Gebdatum) FROM Schueler s))) AS DiffZuAeltester
FROM Schueler s;
```

| S_Nr | S_Zuname   | S_Vorname | DiffZuAeltester |
| ---- | ---------- | --------- | --------------- |
| 1000 | Cartwright | Jaime     | 6642            |
| 1001 | Bogan      | Stanley   | 6829            |
| 1002 | Mertz      | Andy      | 6387            |
| ...  | ...        | ...       | ...             |


## Unterabfragen in Filterkriterien (WHERE und HAVING)

Möchten wir alle Schüler ausgeben, die im selben Jahr wie der älteste Schüler geboren sind, so
verwenden wir unsere Unterabfrage einfach in *WHERE*. Die Funktion *STRFTIME()* ist speziell für
SQLite und gibt Teile (hier das Jahr) des Datumswertes zurück. Beachte die Klammerung der
Unterabfrage im Argument von *STRFTIME*!

**Oracle**
```sql
SELECT   s.S_Nr, s.S_Zuname, s.S_Vorname, s.S_Klasse, s.S_Gebdatum
FROM     Schueler s
WHERE    EXTRACT(YEAR FROM s.S_Gebdatum) = EXTRACT(YEAR FROM (SELECT MIN(s.S_Gebdatum) FROM Schueler s))
ORDER BY s.S_Klasse, s.S_Nr;
```

**SQLite**
```sql
SELECT   s.S_Nr, s.S_Zuname, s.S_Vorname, s.S_Klasse, s.S_Gebdatum
FROM     Schueler s
WHERE    STRFTIME('%Y', s.S_Gebdatum) = STRFTIME('%Y', (SELECT MIN(s.S_Gebdatum) FROM Schueler s))
ORDER BY s.S_Klasse, s.S_Nr;
```

| S_Nr | S_Zuname | S_Vorname | S_Klasse | S_Gebdatum |
| ---- | -------- | --------- | -------- | ---------- |
| 1935 | Kuhic    | Natalie   | 1AO      | 1973-11-19 |
| 1937 | West     | Courtney  | 1AO      | 1973-10-30 |
| 1942 | Towne    | Kevin     | 1AO      | 1973-11-11 |

Nun wollen wir die Klassen herausfinden, wo der älteste Schüler der Klasse im selben Jahr wie der
älteste Schüler der 3BAIF geboren wurde. Im Gegensatz zur vorigen Abfrage wird jetzt jede Klasse
nur 1x ausgegeben.

**Oracle**
```sql
SELECT   s.S_Klasse, MIN(s.S_Gebdatum) AS Aelterster
FROM     Schueler s
GROUP BY s.S_Klasse
HAVING   EXTRACT(YEAR FROM MIN(s.S_Gebdatum)) =
         EXTRACT(YEAR FROM (SELECT MIN(s.S_Gebdatum)
                            FROM Schueler s
                            WHERE s.S_Klasse = '3BAIF'));
```

**SQLite**
```sql
SELECT   s.S_Klasse, MIN(s.S_Gebdatum) AS Aelterster
FROM     Schueler s
GROUP BY s.S_Klasse
HAVING   STRFTIME('%Y', MIN(s.S_Gebdatum)) =
         STRFTIME('%Y', (SELECT MIN(s.S_Gebdatum)
                            FROM Schueler s
                            WHERE s.S_Klasse = '3BAIF'));
```

| S_Klasse | Aelterster |
| -------- | ---------- |
| 1BVIF    | 1989-10-21 |
| 1DVIF    | 1989-11-17 |
| 3AKIF    | 1989-10-13 |
| 3AKKUI   | 1989-10-01 |
| 3BAIF    | 1989-09-10 |
| 5ABKUF   | 1989-11-15 |
| 5ACMNA   | 1989-12-04 |
| 5AKKUI   | 1989-12-11 |
| 5CAIF    | 1989-12-18 |

> **Zusammenfassung:** Unterabfragen, die einen Wert liefern, lassen sich wie Variablen behandeln.
> Sie können überall dort eingesetzt werden, wo Spalten oder fixe Werte stehen können.

## Übungen

Bearbeiten Sie die folgenden Abfragen. Die korrekte Lösung ist in der Tabelle darunter. Die
Bezeichnung der Spalten, die Formatierung und die Sortierung muss nicht exakt übereinstimmen.

**(1)** Welche Lehrer sind neu bei uns, haben also das maximale Eintrittsjahr?

| L_NR | L_NAME      | L_VORNAME  | L_EINTRITTSJAHR |
| ---- | ----------- | ---------- | --------------- |
| BAE  | Blaschke    | Waltraude  | 2021            |
| BIN  | Binder      | Florian    | 2021            |
| BW   | Bergmann    | Wolfgang   | 2021            |
| CEC  | Celeda      | Claus      | 2021            |
| DOM  | Dolezal     | Michael    | 2021            |
| EN   | Engleitner  | Thomas     | 2021            |
| KUA  | Kulich      | Anna-Leena | 2021            |
| LAN  | Langer      | Uwe        | 2021            |
| LES  | Lenk        | Stefan     | 2021            |
| LOY  | Loy         | Amelie     | 2021            |
| MIP  | Michel      | Philip     | 2021            |
| PK   | Pollack-Drs | Susanne    | 2021            |
| POD  | Poppel      | Dominik    | 2021            |
| PRW  | Pramel      | Werner     | 2021            |
| REM  | Rentsch     | Michaela   | 2021            |
| SPN  | Spanner     | Christian  | 2021            |
| SUN  | Subotic     | Nenad      | 2021            |
| SWA  | Schwaiger   | Michael    | 2021            |
| TOM  | Tomc        | Angela     | 2021            |
| TT   | Tschernko   | Thomas     | 2021            |
| VOR  | Voracek     | Adolf      | 2021            |

**(2)** Geben Sie die Klassen der Abteilung AIF und die Anzahl der gesamten Klassen und Schüler der Schule aus.

| K_NR  | KLASSENGESAMT | SCHUELERGESAMT |
| ----- | ------------- | -------------- |
| 2AAIF | 125           | 2592           |
| 2BAIF | 125           | 2592           |
| 2CAIF | 125           | 2592           |
| 2DAIF | 125           | 2592           |
| 3AAIF | 125           | 2592           |
| 3BAIF | 125           | 2592           |
| 3CAIF | 125           | 2592           |
| 4AAIF | 125           | 2592           |
| 4BAIF | 125           | 2592           |
| 4CAIF | 125           | 2592           |
| 5AAIF | 125           | 2592           |
| 5BAIF | 125           | 2592           |
| 6AAIF | 125           | 2592           |
| 6BAIF | 125           | 2592           |

**(3)** Geben Sie bei allen Lehrern, die 2018 eingetreten sind (Spalte *L_Eintrittsjahr*), das Durchschnittsgehalt
(gerechnet über alle Lehrer der Schule) aus.

| L_NR | L_NAME        | L_VORNAME  | L_EINTRITTSJAHR | L_GEHALT | AVGGEHALT |
| ---- | ------------- | ---------- | --------------- | -------- | --------- |
| CAM  | Camrda        | Christian  | 2018            | 2349     | 3099.2    |
| EIA  | Eibler        | Alexander  | 2018            | 2287     | 3099.2    |
| GRL  | Grottenthaler | Lisa       | 2018            |          | 3099.2    |
| LAD  | Latsch        | Daniela    | 2018            | 2151     | 3099.2    |
| MC   | Marek         | Clemens    | 2018            | 2344     | 3099.2    |
| MUE  | Müller        | Simone     | 2018            | 2335     | 3099.2    |
| NWV  | Neuwirth      | Veronika   | 2018            |          | 3099.2    |
| PC   | Pemmer        | Christian  | 2018            | 2150     | 3099.2    |
| RRN  | Rodas Reyna   | Fitzgerald | 2018            |          | 3099.2    |
| SJ   | Strassl       | Johannes   | 2018            |          | 3099.2    |
| STC  | Stach         | Angela     | 2018            |          | 3099.2    |
| TOF  | Tonti         | Fabio      | 2018            | 2166     | 3099.2    |
| ZOE  | Zöttl         | Andreas    | 2018            |          | 3099.2    |

**(4)** Als Ergänzung geben Sie nun bei diesen Lehrern die Abweichung vom Durchschnittsgehalt
aus. Zeigen Sie dabei nur die Lehrer an, die weniger als 800 Euro unter dem Durchschnittswert verdienen.

| L_NR | L_NAME | L_VORNAME | L_EINTRITTSJAHR | L_GEHALT | AVGGEHALT | ABWEICHUNG |
| ---- | ------ | --------- | --------------- | -------- | --------- | ---------- |
| EIA  | Eibler | Alexander | 2018            | 2287     | 3099.2    | -812.16    |
| LAD  | Latsch | Daniela   | 2018            | 2151     | 3099.2    | -948.16    |
| PC   | Pemmer | Christian | 2018            | 2150     | 3099.2    | -949.16    |
| TOF  | Tonti  | Fabio     | 2018            | 2166     | 3099.2    | -933.16    |

**(5)** Geben Sie die Prüfungen aus, die maximal 3 Tage (72 Stunden) vor der letzten Prüfung stattfanden.

| P_DATUMZEIT             | P_PRUEFER | P_NOTE | S_ZUNAME    | S_VORNAME |
| ----------------------- | --------- | ------ | ----------- | --------- |
| 2022-05-29 10:25:00.000 | MOS       |        | Welch       | Janie     |
| 2022-05-29 13:50:00.000 | BAR       | 5      | Kling       | Cody      |
| 2022-05-29 14:15:00.000 | LUP       |        | Hagenes     | Kirk      |
| 2022-05-29 16:50:00.000 | PF        | 5      | Gibson      | Gary      |
| 2022-05-30 08:10:00.000 | BOM       |        | Koch        | Kevin     |
| 2022-05-30 08:30:00.000 | SCM       | 2      | Harvey      | Carla     |
| 2022-05-30 09:20:00.000 | GRM       | 3      | Towne       | Andrea    |
| 2022-05-30 13:15:00.000 | AT        |        | Botsford    | Jimmy     |
| 2022-05-30 20:30:00.000 | AF        | 3      | Satterfield | Clay      |
| 2022-05-31 10:20:00.000 | SOG       |        | Kris        | Valerie   |
| 2022-05-31 11:20:00.000 | STC       | 5      | Schroeder   | Melba     |
| 2022-05-31 13:20:00.000 | KUA       | 2      | Prohaska    | Adrian    |
| 2022-05-31 13:25:00.000 | ZEP       | 4      | Dare        | Lance     |
| 2022-05-31 18:55:00.000 | BRA       | 2      | Wunsch      | Jan       |
| 2022-05-31 19:45:00.000 | AT        | 2      | D'Amore     | Nicholas  |
| 2022-05-31 21:40:00.000 | BH        | 2      | Conn        | Mitchell  |


**(6)** Geben Sie die Räume mit der meisten Kapazität (Spalte *R_Plaetze*) aus. Hinweis: Das können auch
mehrere Räume sein.

| R_ID  | R_PLAETZE | R_ART                        |
| ----- | --------- | ---------------------------- |
| AH.32 | 36        | Naturwissenschaftlicher Raum |
| C1.13 | 36        | Klassenraum                  |

**(7)** Gibt es Räume, die weniger als ein Viertel der Plätze als der größte Raum haben?

| R_ID    | R_PLAETZE | R_ART                                     |
| ------- | --------- | ----------------------------------------- |
| AH.21   | 6         | Bibliothek                                |
| BH.08W  | 8         | Netzwerktechnik                           |
| BH.09aW | 8         | Mechanik                                  |
| BH.09bW | 8         | CNC-Technik                               |
| BH.09cW | 8         | Blechbearbeitung                          |
| BH.09dW | 8         | Schweisstechnik                           |
| BH.10aW | 8         | Plasmaschneider                           |
| BH.11W  | 8         | Mechanik                                  |
| DE.04L  | 8         | Instrumentelle Analytik                   |
| DE.05L  | 8         | Analytik und Prüftechnik                  |
| DE.06L  | 8         | Umweltlabor                               |
| DE.09L  | 8         | Nasschemisches Labor                      |
| DE.10W  | 8         | Reinigungs- und Facilitytechnik           |
| DE.11W  | 8         | Färbe-, Veredlungs- und Verfahrenstechnik |
| DE.12aW | 8         | Farblabor                                 |
| DE.13aW | 8         | Drucktechnik                              |
| DE.15L  | 8         | Versuchsanstalt Prüflabor                 |

**(8)** Welche Klasse hat mehr weibliche Schüler (S_Geschlecht ist 2) als die 5AHBGM? Hinweis: Gruppieren Sie
die Schülertabelle und vergleichen die Anzahl mit dem ermittelten Wert aus der 5AHBGM.

| S_KLASSE | ANZWEIBL |
| -------- | -------- |
| 1AHMNG   | 18       |
| 1AHWIT   | 19       |
| 1BFITM   | 22       |
| 2AHKUI   | 18       |
| 3AAIF    | 17       |
| 3BHBGM   | 19       |
| 3BHWIT   | 19       |
| 5ACMNA   | 20       |
| 5BKIF    | 18       |

**(9)** Geben Sie die Klassen der Abteilung BIF sowie die Anzahl der Schüler in dieser Abteilung aus.
Hinweis: Verwenden Sie GROUP BY, um die Schüleranzahl pro Klasse zu ermitteln. Achten Sie auch
darauf, dass Klassen mit 0 Schülern auch angezeigt werden. Danach schreiben Sie 
eine Unterabfrage, die die Schüler der BIF Abteilung zählt.

| K_NR  | SCHUELERKLASSE | SCHUELERBIF |
| ----- | -------------- | ----------- |
| 2ABIF | 0              | 108         |
| 3ABIF | 30             | 108         |
| 4ABIF | 0              | 108         |
| 5ABIF | 25             | 108         |
| 6ABIF | 0              | 108         |
| 7ABIF | 24             | 108         |
| 7BBIF | 29             | 108         |
| 8ABIF | 0              | 108         |
| 8BBIF | 0              | 108         |
