# Historisierung (Temporale Datenhaltung)

> Unter temporaler Datenhaltung (auch Historisierung genannt) versteht man in der
> Informationstechnik das Festhalten der zeitlichen Entwicklung der Daten bei Speicherung in einer Datenbank.
> <sup>https://de.wikipedia.org/wiki/Temporale_Datenhaltung</sup>

Wir betrachten das Modell einer kleinen Datenbank für eine Tankstelle. Diese Tankstelle bietet
verschiedene Kategorien von Treibstoffen zu einem bestimmten Preis an. Die Verkäufe werden mit
der verkauften Menge in Litern gespeichert.

![](tankstelle_ohne_history_1.png)

Nun ändert sich der Benzinpreis. Der Wert wird also mit einem *UPDATE* Statement neu gesetzt. Doch
was passiert mit unseren gespeicherten Verkäufen? Da in der Tabelle *Verkauf* nur auf den Preis
*referenziert* wird, hat sich auch der Jahresumsatz durch die Preisänderung verändert.

In der Datenmodellierung gibt es häufig Fälle, wo der Zustand zu einem bestimmten Zeitpunkt in
der Vergangenheit abgefragt wird:

- Welchen Lagerstand hatte ein Produkt im Laufe der Zeit?
- Welches Gehalt hatte ein Mitarbeiter?
- Welche Vertragskonditionen (Tarif) hat ein Kunde, der zu einem bestimmten Zeitpunkt den Vertrag
  abgeschlossen hat?
- Wer hat Datenkorrekturen (z. B. Noteneintragung) durchgeführt und was war der vorherige Wert?  

Als Faustregel kann man im Allgemeinen sagen: *INSERT* Anweisungen sind besser als *UPDATE*
Anweisungen, denn sie überschreiben keine alten Daten. Verwenden Sie nur *UPDATE*, wenn Sie den
alten Stand der Daten nicht mehr benötigen.

Ein einfacher Ansatz, um den Preis eines Verkaufes in der Vergangenheit herauszufinden, wäre das
zusätzliche Speichern in der Tabelle Verkauf:

![](tankstelle_history_1.png)

Die Tabelle Preis beinhaltet den aktuellen Wert, der auf der Homepage oder der Zapfsäule angezeigt
wird. Bei einem Verkauf wird dieser Wert übernommen. Wollen Sie allerdings den Preisverlauf als
Diagramm darstellen, ergibt sich allerdings ein Problem:

- Von wann bis wann galt dieser Preis? Sie sehen nur das Verkaufsdatum, wann ein Kunde getankt hat.
- Was passiert, wenn kein Kunde zu diesem Preis getankt hat (z. B. Sonntag)?

Diese Überlegungen können wir nur mit einer Überarbeitung der Preistabelle lösen. Wir speichern nun
ein *Zeitintervall*, wann der Preis gültig ist.

![](tankstelle_history_2.png)

Für diese Speicherung müssen Sie allerdings einiges beachten:

- Der letzte (aktuelle) Preis hat als Attribut *GueltigBis* den Wert NULL oder ein hohes Datum
  (z. B. 31.12.2999).
- Beim Einfügen eines neuen Preises muss *GueltigBis* des vorher aktuellen Preises begrenzt werden.
- Die Intervalle dürfen keine Lücken aufweisen.

Genaugenommen können Sie auch auf *GueltigBis* verzichten, da die Intervalle ja immer anschließend
sind. Dies würde die Abfragen allerdings komplizierter machen, da Sie mit einer Unterabfrage einmal
den Beginn des Intervalls mit *MAX(GueltigVon) WHERE GueltigVon <= :Zieldatum* herausfinden müssen.

## Abfragebeispiele

In der SQLite Datenbank [Tankstellen.db](Tankstellen.db) sind Musterdaten mit dem oben gezeigten
Modell gespeichert. Wir fragen nun verschiedene Sachen ab.

Folgende Daten sind in der Tabelle *Tankstelle*:

| Id  | Adresse           | Plz  | Ort                |
| --- | ----------------- | ---- | ------------------ |
| 1   | 5908 Ratke Loop   | 5886 | New Dock           |
| 2   | 14207 Huels Point | 1265 | South Lillianaberg |

Folgende Daten sind in der Tabelle *Kategorie*:

| Id  | Name          |
| --- | ------------- |
| 1   | Benzin super  |
| 2   | Benzin normal |
| 3   | Diesel        |

### Preis zum Zeitpunkt *t*

Die Preise für die Tankstelle 2 für Benzin super (Kategorie 1) sind wie folgt abzufragen:

```sql
SELECT *
FROM Preis p
WHERE p.KategorieId = 1 AND p.TankstelleId = 2
ORDER BY p.GueltigVon;
```

| Id   | Wert   | GueltigVon | GueltigBis | TankstelleId | KategorieId |
| :--- | :----- | :--------- | :--------- | :----------- | :---------- |
| 2    | 1.3000 | 2019-01-11 | 2019-02-10 | 2            | 1           |
| 5    | 1.2926 | 2019-02-10 | 2019-03-12 | 2            | 1           |
| 8    | 1.2943 | 2019-03-12 | 2019-04-01 | 2            | 1           |
| 10   | 1.3035 | 2019-04-01 | 2019-04-21 | 2            | 1           |
| 12   | 1.3287 | 2019-04-21 | 2019-05-01 | 2            | 1           |
| 13   | 1.3423 | 2019-05-01 | 2019-05-31 | 2            | 1           |
| 16   | 1.3342 | 2019-05-31 | 2019-07-10 | 2            | 1           |
| 20   | 1.3327 | 2019-07-10 | NULL       | 2            | 1           |

Wollen wir nun wissen, welche Preise für die Tankstelle 2 am 1. April 2019 eingetragen
wurden, fragen wir mit 2 Kriterien ab:

```sql
SELECT *
FROM Preis p
WHERE
    p.TankstelleId = 2 AND
    p.GueltigVon <= '2019-04-01' AND p.GueltigBis > '2019-04-01'
ORDER BY p.KategorieId;
```

| Id   | Wert   | GueltigVon | GueltigBis | TankstelleId | KategorieId |
| :--- | :----- | :--------- | :--------- | :----------- | :---------- |
| 8    | 1.2943 | 2019-03-12 | 2019-04-01 | 2            | 1           |
| 1    | 1.2000 | 2019-01-01 | 2019-05-11 | 2            | 2           |
| 3    | 1.1000 | 2019-01-21 | 2019-05-21 | 2            | 3           |

Beachten Sie, dass wir für *GueltigVon* den Operator <= verwenden und für *GueltigBis* den
Operator >.

Wir versuchen nun, den Preis für den 1. November 2019 nach derselben Methode herauszufinden.
Auf einmal sehen wir nur mehr 1 Datensatz im Ergebnis:

| Id   | Wert   | GueltigVon | GueltigBis | TankstelleId | KategorieId |
| :--- | :----- | :--------- | :--------- | :----------- | :---------- |
| 31   | 1.1681 | 2019-10-28 | 2019-11-07 | 2            | 2           |

Das Problem ist der Wert NULL, wenn der Preis aktuell gültig ist. In der Kategorie 1 und 3 hat
der Preis, der am 1.11.2019 gilt, kein Enddatum, da er noch gilt. Da NULL aber beim Vergleich mit
dem > Operator nicht true liefert, wird er ausgefiltert.

Mit *COALESCE* können wir das Problem lösen, indem wir ein hohes Datum in der Zukunft für unseren
NULL wert zurückgeben lassen.

```c#
SELECT *
FROM Preis p
WHERE
    p.TankstelleId = 2 AND
    p.GueltigVon <= '2019-11-01' AND COALESCE(p.GueltigBis, DATE('2099-12-31')) > '2019-11-01';
```

| Id   | Wert   | GueltigVon | GueltigBis | TankstelleId | KategorieId |
| :--- | :----- | :--------- | :--------- | :----------- | :---------- |
| 20   | 1.3327 | 2019-07-10 | NULL       | 2            | 1           |
| 31   | 1.1681 | 2019-10-28 | 2019-11-07 | 2            | 2           |
| 30   | 1.0758 | 2019-10-18 | NULL       | 2            | 3           |

Das Abfragen des gerade gültigen Preises ist mit der Information, dass *GueltigBis* im letzten
Intervall den Wert NULL hat, sehr einfach:

```c#
SELECT *
FROM Preis p
WHERE p.TankstelleId = 2 AND p.GueltigBis IS NULL
ORDER BY p.KategorieId;
```

| Id   | Wert   | GueltigVon | GueltigBis | TankstelleId | KategorieId |
| :--- | :----- | :--------- | :--------- | :----------- | :---------- |
| 20   | 1.3327 | 2019-07-10 | NULL       | 2            | 1           |
| 37   | 1.1931 | 2019-12-27 | NULL       | 2            | 2           |
| 30   | 1.0758 | 2019-10-18 | NULL       | 2            | 3           |

## Übung

**(1)** In der Tankstellendatenbank gibt es eine Tabelle *Tag*. Diese ist sehr einfach gebaut: Sie
speichert einfach jeden einzelnen Tag des 21. Jahrhunderts (1.1.2000 - 31.12.2099). Schreiben
Sie eine Abfrage, die den Tagespreis für Diesel (Kategorie 3) im Jahr 2019 an der
Tankstelle 1 ausgibt. Hinweis: Da nicht für das ganze Jahr eine Preis vorhanden ist, wird
Ihr Ergebnis erst nach dem 1. Jänner beginnen. Es muss aber bis 31. Dezember 2019 gehen, da hier
der letzte eingetragene Preis verwendet wird.

**(2)** Ermitteln Sie mit einem SQL Statement den Durchschnittspreis im Juli 2019 für Diesel
(Kategorie 3). Beachten Sie, dass bei der Berechnung des Durchschnittes die einzelnen Tage
verwendet werden, um jeden Tag des Monats - egal wie lange der Preis gilt - gleich stark zu
gewichten. Prüfen Sie Ihre Antwort, indem Sie die Tagespreise in Excel laden und den Mittelwert
dort berechnen.

**(3)** Schreiben Sie Ihre Abfrage von (1) so um, dass Ihr Ergebnis mit 1. Jänner 2019
beginnt. Falls noch kein Preis vorhanden ist, soll NULL geliefert werden. Hinweis: Verwenden
Sie *LEFT JOIN* und filtern Sie nach der Tankstelle im JOIN Ausdruck. Begründen Sie, warum das
Filtern nach der Tankstelle im WHERE Ausdruck Ihre Ergebnisse am Anfang des Jahres wieder entfernt.

**(3)** In unserer Schuldatenbank sind Schüler (Vor- und Zuname sowie Geburtsdatum) einer Klasse
(Bezeichnung und Stammraum) zugeordnet. Erstellen Sie ein kleines Modell, bei dem Sie herausfinden
können, welche Klasse jeder Schüler in jedem Schuljahr besucht hat.

**(4)** Verbinden Sie mit der Schülertabelle aus Übung (3) eine Tabelle Zeugnisnoten. Diese
Zeugnisnoten werden von einem Lehrer (Vor- und Zuname) in einem Fach (Bezeichnung und Langname)
für eine bestimmtes Schuljahr eingetragen. Anstatt die Note einfach zu überschreiben, wollen
Sie ein Protokoll haben, wer die Note geändert hat. Wie finden Sie die aktuelle Note heraus? Geben
Sie die SQL Abfrage hierfür an.
