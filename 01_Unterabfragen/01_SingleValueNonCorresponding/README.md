# Nicht korrespondierende Unterabfragen, die einen Wert liefern.

**(1)** Geben Sie die Klassen der Abteilung AIF und die Anzahl der gesamten Klassen und Schüler der Schule aus.

| #   | Klasse | KlassenGesamt | SchuelerGesamt |
| ---:| ------ | -------------:| --------------:|
|   1 | 2AAIF  | 61            | 1138           |
|   2 | 2BAIF  | 61            | 1138           |
|   3 | 2CAIF  | 61            | 1138           |
|   4 | 2DAIF  | 61            | 1138           |
|   5 | 3BAIF  | 61            | 1138           |
|   6 | 3CAIF  | 61            | 1138           |
|   7 | 4BAIF  | 61            | 1138           |
|   8 | 4CAIF  | 61            | 1138           |
|   9 | 5BAIF  | 61            | 1138           |
|  10 | 5CAIF  | 61            | 1138           |
|  11 | 6BAIF  | 61            | 1138           |
|  12 | 6CAIF  | 61            | 1138           |

**(2)** Geben Sie allen Lehrern, die 2018 eingetreten sind (Spalte *L_Eintrittsjahr*), das Durchschnittsgehalt aus.

| #   | LNr | LName     | LVorname  | LEintrittsjahr | LGehalt | AvgGehalt |
| ---:| --- | --------- | --------- | --------------:| -------:| ---------:|
|   1 | AH  | Auinger   | Harald    | 2018           | 2083    | 3126.67   |
|   2 | BIE | Bierbamer | Peter     | 2018           | 2225    | 3126.67   |
|   3 | CAM | Camrda    | Christian | 2018           |         | 3126.67   |
|   4 | HY  | Horny     | Christian | 2018           | 2224    | 3126.67   |
|   5 | KEM | Keminger  | Alexander | 2018           | 2138    | 3126.67   |
|   6 | KMO | Kmyta     | Olga      | 2018           | 2122    | 3126.67   |
|   7 | MC  | Marek     | Clemens   | 2018           | 2158    | 3126.67   |
|   8 | PEC | Pemöller  | Christoph | 2018           |         | 3126.67   |
|   9 | SE  | Schmid    | Erhard    | 2018           | 2064    | 3126.67   |
|  10 | ZLA | Zlabinger | Walter    | 2018           | 2256    | 3126.67   |

**(3)** Als Ergänzung geben Sie nun bei diesen Lehrern die Abweichung vom Durchschnittsgehalt
aus. Zeigen Sie dabei nur die Lehrer an, über 1000 Euro unter diesem Durchschnittswert verdienen.

| #   | LNr | LName   | LVorname | LEintrittsjahr | LGehalt | AvgGehalt | Abweichung |
| ---:| --- | ------- | -------- | --------------:| -------:| ---------:| ----------:|
|   1 | AH  | Auinger | Harald   | 2018           | 2083    | 3126.67   | -1043.67   |
|   2 | KMO | Kmyta   | Olga     | 2018           | 2122    | 3126.67   | -1004.67   |
|   3 | SE  | Schmid  | Erhard   | 2018           | 2064    | 3126.67   | -1062.67   |

**(4)** Geben Sie die Prüfungen aus, die maximal 3 Tage vor der letzten Prüfung stattfanden.

| #   | PDatumZeit          | PPruefer | PNote | Zuname    | Vorname |
| ---:| ------------------- | -------- | -----:| --------- | ------- |
|   1 | 31.05.2020 17:10:00 | SWA      | 2     | Abernathy | Jeanne  |
|   2 | 29.05.2020 08:55:00 | KOL      | 5     | Bartell   | Jimmie  |
|   3 | 29.05.2020 08:05:00 | CO       | 2     | Bogan     | Richard |

**(5)** Geben Sie die Räume mit der meisten Kapazität (Spalte *R_Plaetze*) aus. Hinweis: Das können auch
mehrere Räume sein.

| #   | RId   | RPlaetze | RArt                         |
| ---:| ----- | --------:| ---------------------------- |
|   1 | AH.32 | 36       | Naturwissenschaftlicher Raum |
|   2 | B5.09 | 36       | Klassenraum                  |

**(6)** Gibt es Räume, die unter einem Viertel der Plätze als der größte Raum haben?

| #   | RId     | RPlaetze | RArt                                      |
| ---:| ------- | --------:| ----------------------------------------- |
|   1 | A3.04   | 3        | Multifunktionsraum Medien                 |
|   2 | AH.21   | 6        | Bibliothek                                |
|   3 | BH.08W  | 8        | Maschentechnik                            |
|   4 | BH.09aW | 8        | Gewebetechnik                             |
|   5 | BH.10   | 8        | Schweisstechnik                           |
|   6 | BH.11W  | 8        | Mechanik                                  |
|   7 | BLA     | 8        | Betriebslaboratorium                      |
|   8 | DE.04L  | 8        | Instrumentelle Analytik                   |
|   9 | DE.05L  | 8        | Analytik und Prüftechnik                  |
|  10 | DE.06L  | 8        | Umweltlabor                               |
|  11 | DE.09L  | 8        | Nasschemisches Labor                      |
|  12 | DE.10W  | 8        | Reinigungs- und Facilitytechnik           |
|  13 | DE.11W  | 8        | Färbe-, Veredlungs- und Verfahrenstechnik |
|  14 | DE.12aW | 8        | Farblabor                                 |
|  15 | DE.13aW | 8        | Drucktechnik                              |
|  16 | DE.15L  | 8        | Versuchsanstalt Prüflabor                 |
|  17 | DE.19L  | 8        | Versuchsanstalt Grünbereich               |

**(7)** Welche Klasse hat mehr weibliche Schüler (S_Geschlecht ist 2) als die 5BAIF? Hinweis: Gruppieren Sie
die Schülertabelle und vergleichen die Anzahl mit dem ermittelten Wert aus der 5BAIF.

| #   | Klasse | AnzWeibl |
| ---:| ------ | --------:|
|   1 | 5BBIF  | 17       |
|   2 | 7CBIF  | 17       |

**(8)** Geben Sie die Klassen der Abteilung BIF sowie die Anzahl der Schüler in dieser Abteilung aus.
Hinweis: Verwenden Sie GROUP BY, um die Schüleranzahl pro Klasse zu ermitteln. Achten Sie auch
darauf, dass Klassen mit 0 Schülern auch angezeigt werden. Danach schreiben Sie 
eine Unterabfrage, die die Schüler der BIF Abteilung zählt.

| #   | Klasse | SchuelerKlasse | SchuelerBIF |
| ---:| ------ | --------------:| -----------:|
|   1 | 2ABIF  | 0              | 110         |
|   2 | 3BBIF  | 27             | 110         |
|   3 | 4BBIF  | 0              | 110         |
|   4 | 5BBIF  | 27             | 110         |
|   5 | 6BBIF  | 0              | 110         |
|   6 | 7BBIF  | 28             | 110         |
|   7 | 7CBIF  | 28             | 110         |
|   8 | 8BBIF  | 0              | 110         |
|   9 | 8CBIF  | 0              | 110         |
