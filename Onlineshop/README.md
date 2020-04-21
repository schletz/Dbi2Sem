# Aufgabenstellung zu Unterabfragen

## Datenbank und Schema

In der Datei [Shop.db](Shop.db) befindet sich eine SQLite Datenbank, die Verkäufe aus einem Onlineshop
simuliert. Diese Datenbank hat folgendes Schema:

![](schema.png)

- Die Tabelle *Kunden* speichert alle registrierten Kunden. Das Bundesland wird mit dem Kürzel gespeichert
  und hat im Datenbestand die Werte *B*, *N* oder *W*.
- Kunden können mehrere Bestellungen aufgeben. Diese Bestellung hat ein Bestelldatum und verweist auf
  den Kunden, der sie aufgegeben hat.
- Die Bestellung umfasst mehrere Positionen. In der Position wird der Artikel der Bestellung
  zugeordnet. Auch die Menge der bestellten Artikel wird dort gespeichert.
- Der Artikel wird einer Kategorie zugeordnet und hat einen Preis.

## Bewertung und Abgabe

Jede korrekt gelöste Aufgabe bekommt 1 Punkt. Eine Aufgabe gilt als korrekt gelöst, wenn

- Die ausgegebenen Datensätze mit der Musterlösung übereinstimmen. Formatierung und Sortierung
  müssen allerdings nicht beachtet werden, Spaltennamen jedoch schon.
- Bei Zahlen kann es durch die Maschinengenauigkeit zu leichten Abweichungen
  in der letzten Kommastelle kommen (statt 0.1 wird 0.099999999 ausgegeben). Das ist in Ordnung.  
- Die Abfrage allgemeingültig ist, also keine fix eingetragenen Werte hat oder Sonderfälle, die
  nur in diesem Datenbestand zutreffen, verwendet.

Falls Sie den Eindruck haben, dass die Musterausgabe nicht korrekt ist, vermerken Sie bitte
Ihre Argumentation in der Abgabedatei. Dies wird dann berücksichtigt.

Verwenden Sie das untenstehende Muster für Ihre SQL Datei und schreiben Sie Ihren Namen
hinein. Schreiben Sie Ihre Lösung unter den entsprechenden Kommentar mit der Aufgabennummer.
Geben Sie diese Datei in Teams bis 22. April 2020 um 12:00 ab.

Würde die Aufgabenstellung separat benotet werden, ergibt sich - zu Ihrer Selbteinschätzung -
folgende Skala: 12 Punkte: 1, 11 - 10 Punkte: 2, 9 - 8 Punkte: 3, 7 Punkte: 4

## Aufgaben

**(1)** Geben Sie die teuersten Artikel aus, die in der Tabelle Artikel gespeichert sind.

|ArtikelId|EAN|Name|Preis|KategorieId|
|---------|---|----|-----|-----------|
|5|4070396806834|Refined Soft Fish|226|3|
|8|3948358760443|Awesome Concrete Sausages|226|4|
|10|3971100666748|Unbranded Steel Fish|226|4|

**(2)** Geben Sie die teuersten Artikel der Kategorie 4 (KategorieId) aus.

|ArtikelId|EAN|Name|Preis|KategorieId|
|---------|---|----|-----|-----------|
|8|3948358760443|Awesome Concrete Sausages|226|4|
|10|3971100666748|Unbranded Steel Fish|226|4|

**(3)** Welche Artikel in der Kategorie 4 sind teurer als der teuerste Artikel in Kategorie 1?

|ArtikelId|EAN|Name|Preis|KategorieId|
|---------|---|----|-----|-----------|
|8|3948358760443|Awesome Concrete Sausages|226|4|
|10|3971100666748|Unbranded Steel Fish|226|4|
|14|2208113939738|Practical Steel Hat|225.6|4|

**(4)** Fehler bei der Numerierung, dieser Punkt bleibt leer (ist also einfach zu lösen ;).

**(5)** Gibt es "leere" Bestellungen, also Bestellungen ohne Positionen? Hinweis: Am Einfachsten
        funktioniert dies mit *NOT EXISTS*.

|BestellungId|Datum|KundeId|
|------------|-----|-------|
|4|2020-01-01 06:32:01.87|4|
|11|2020-01-19 03:49:56.68|14|
|23|2020-01-12 12:48:02.591|9|
|26|2020-01-04 18:32:28.25|8|
|44|2020-01-05 22:26:16.437|2|
|46|2020-01-17 07:00:51.574|14|
|61|2020-01-15 23:59:09.545|10|

**(6)** Welche Artikel aus der Kategorie 4 (*KategorieId*) wurden von Kunden aus Niederösterreich
        (*Kunde.Bundesland* ist *N*) gekauft? Gehen Sie dafür von den Positionen zu den Bestellungen
        und dann zum Kunden.

|ArtikelId|EAN|Name|Preis|KategorieId|
|---------|---|----|-----|-----------|
|1|9193237222237|Sleek Steel Pizza|214.49|4|
|7|8907008028048|Licensed Concrete Fish|210.45|4|
|10|3971100666748|Unbranded Steel Fish|226|4|
|14|2208113939738|Practical Steel Hat|225.6|4|

**(7)** Welche Produkte wurden niemals von Kunden aus Niederösterreich gekauft?

|ArtikelId|EAN|Name|Preis|KategorieId|
|---------|---|----|-----|-----------|
|8|3948358760443|Awesome Concrete Sausages|226|4|

**(8)** Listen Sie alle Bestellungen auf, die Artikel mit der KategorieId 3, aber nicht mit der
         KategorieId 4 enthalten.

|BestellungId|Datum|KundeId|
|------------|-----|-------|
|1|2020-01-15 08:42:30.368|5|
|9|2020-01-02 15:33:18.882|14|
|12|2020-01-10 10:30:38.446|9|
|19|2020-01-19 08:58:03.801|7|
|21|2020-01-17 19:18:07.329|2|
|29|2020-01-15 00:12:39.008|10|
|30|2020-01-10 15:37:28.249|6|
|31|2020-01-08 12:49:00.842|7|
|41|2020-01-05 06:54:04.25|16|
|45|2020-01-09 17:49:39.51|9|
|50|2020-01-03 08:32:28.515|9|
|57|2020-01-02 16:10:33.496|7|

**(9)** Welche Bestellungen umfassen nur Artikel der KategorieId 3? Achtung:
        Bestellungen ohne Positionen sollen hierfür ausgeschlossen werden.

|BestellungId|Datum|KundeId|
|------------|-----|-------|
|9|2020-01-02 15:33:18.882|14|
|19|2020-01-19 08:58:03.801|7|
|41|2020-01-05 06:54:04.25|16|
|57|2020-01-02 16:10:33.496|7|

**(10)** Erstellen Sie eine View *vUmstatzstatistik*, die pro Kunde den Umsatz aufsummiert. Sie
         berechnen den Umsatz einer Position mit Menge x Artikelpreis. Die nachfolgenden Daten
         entstehen bei der Abfrage `SELECT * FROM vUmsatzstatistik`.

|KundeId|Vorname|Zuname|Bundesland|Kundenumsatz|
|-------|-------|------|----------|------------|
|1|Dedric|Shanahan|W|19330.809999999998|
|2|Alaina|Bashirian|N|11156.380000000001|
|3|Johan|Weimann|N|11886.899999999998|
|4|Taylor|Metz|W|13368.13|
|5|Rebekah|Morar|W|6035.71|
|6|Holden|Mohr|B|3303.9|
|7|Bethel|Corwin|W|5360.05|
|9|Rachael|Walter|B|17451.41|
|10|Brianne|Dare|N|3687.8199999999997|
|11|Jaron|Pagac|B|4563|
|12|Merl|Ryan|B|15122.74|
|13|Prince|Mitchell|N|7890.17|
|14|Kara|Gorczany|W|9996.85|
|15|Jailyn|Stokes|B|5754.11|
|16|Elliot|Vandervort|B|4330.45|

**(11)** Geben Sie mit Hilfe dieser View den umsatzstärksten Kunden pro Bundesland aus. Beachten
         Sie, dass es auch mehrere Kunden pro Bundesland geben kann, die diesen Umsatz generieren.

|Bundesland|Vorname|Zuname|KundeId|Kundenumsatz|
|----------|-------|------|-------|------------|
|W|Dedric|Shanahan|1|19330.81|
|N|Johan|Weimann|3|11886.9|
|B|Rachael|Walter|9|17451.41|

**(12)** Erstellen Sie mit Hilfe dieser View die Umsatzstatistik pro Bundesland.

|Bundesland|Umsatz|
|----------|------|
|B|50525.61|
|N|34621.27|
|W|54091.549999999996|

## Vorlage für die SQL Datei

```sql
-- ÜBUNG ZU SQL UNTERABFRAGEN
-- ZUNAME VORNAME
-- 4CAIF, 21. und 22. Apr 2020

-- Aufgabe 1

-- Aufgabe 2

-- Aufgabe 3

-- Aufgabe 4

-- Aufgabe 5

-- Aufgabe 6

-- Aufgabe 7

-- Aufgabe 8

-- Aufgabe 9

-- Aufgabe 10

-- Aufgabe 11

-- Aufgabe 12
```
