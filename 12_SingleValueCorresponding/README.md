# Korrespondierende Unterabfragen, die einen Wert liefern

Bis jetzt haben unsere Unterabfragen keinen Wert von der äußeren Abfrage bezogen. Es gibt allerdings
Situationen, in denen das notwendig ist. Wir betrachten die folgende Abfrage, bei der wir ermitteln,
wie viele Abteilungen ein Abteilungsvorstand leitet:

![](corresponding_query.png)

Da hier die Abteilungstabelle 2x verwendet wird, ist der Alias *a* und *a2* notwendig. Für das
Verständnis ist es hilfreich, sich die äußere Abfrage als Schleife vorzustellen, die durch die
einzelnen Werte geht. Für jeden Wert wird dann eine Funktion (hier *MySubQuery*) aufgerufen.

```c#
foreach (Abteilung a in db.Abteilungen)
{
    var value = MySubQuery(a.AbtLeiter);
    Console.WriteLine($"{a.AbtNr} {a.AbtLeiter} {value}")
}
```

Im vorigen Kapitel stand der Aufruf von *MySubQuery()* *vor* der Schleife. Nun ist er in der Schleife.
Das erklärt schon einige Nachteile der korrespondierenden Unterabfragen:

- Das Ergebnis kann nicht zwischengespeichert werden, da es sich bei jedem Durchlauf ändert.
- Unsere Unterabfrage - also hier die Funktion *MySubQuery()* - kann nicht unabhängig ausgeführt
  und getestet werden, da sie Parameter der äußeren Abfrage braucht.

Wenn Problemstellungen sich als nicht korrespondierende Unterabfragen umsetzen lassen, sollte also
immer dieser Weg gegangen werden.

Es gelten alle Regeln die im vorigen Kapitel behandelt wurden, da sich die Art der Unterabfrage
nicht geändert hat. Sie können daher an Stellen verwendet werden, wo Spalten oder Werte in SQL
stehen können.

## Übungen

Bearbeiten Sie die folgenden Abfragen. Die korrekte Lösung ist in der Tabelle darunter, die erste
Spalte (#) ist allerdings nur die Datensatznummer und kommt im Abfrageergebnis nicht vor. Die
Bezeichnung der Spalten, die Formatierung und die Sortierung muss nicht exakt übereinstimmen.

**(1)** Geben Sie die Klassen der Abteilung HIF und die Anzahl der männlichen und weiblichen Schüler aus.
| #   | Klasse | AnzGesamt | AnzMaennl | AnzWeibl |
| ---:| ------ | ---------:| ---------:| --------:|
|   1 | 1AHIF  | 32        | 24        | 8        |
|   2 | 1BHIF  | 32        | 32        | 0        |
|   3 | 1CHIF  | 32        | 32        | 0        |
|   4 | 1DHIF  | 32        | 32        | 0        |
|   5 | 1EHIF  | 32        | 28        | 4        |
|   6 | 1FHIF  | 32        | 32        | 0        |
|   7 | 2AHIF  | 27        | 21        | 6        |
|   8 | 2BHIF  | 27        | 27        | 0        |
|   9 | 2CHIF  | 26        | 26        | 0        |
|  10 | 2DHIF  | 27        | 27        | 0        |
|  11 | 2EHIF  | 28        | 23        | 5        |
|  12 | 3AHIF  | 26        | 23        | 3        |
|  13 | 3BHIF  | 23        | 23        | 0        |
|  14 | 3CHIF  | 24        | 24        | 0        |
|  15 | 3EHIF  | 25        | 18        | 7        |
|  16 | 4AHIF  | 25        | 22        | 3        |
|  17 | 4BHIF  | 27        | 27        | 0        |
|  18 | 4CHIF  | 27        | 27        | 0        |
|  19 | 4EHIF  | 25        | 17        | 8        |
|  20 | 5AHIF  | 27        | 21        | 6        |
|  21 | 5BHIF  | 27        | 27        | 0        |
|  22 | 5CHIF  | 29        | 29        | 0        |
|  23 | 5EHIF  | 27        | 23        | 4        |

**(2)** In welchen Klassen gibt es mehr weibliche als männliche Schüler?
| #   | Klasse | AnzGesamt | AnzMaennl | AnzWeibl |
| ---:| ------ | ---------:| ---------:| --------:|
|   1 | 1BVIF  | 26        | 12        | 14       |
|   2 | 1DVIF  | 27        | 13        | 14       |
|   3 | 1EVIF  | 27        | 11        | 16       |
|   4 | 3BAIF  | 25        | 10        | 15       |
|   5 | 3BKIF  | 29        | 13        | 16       |
|   6 | 3CAIF  | 27        | 13        | 14       |
|   7 | 5ACIF  | 25        | 12        | 13       |
|   8 | 5BAIF  | 26        | 10        | 16       |
|   9 | 5BBIF  | 27        | 10        | 17       |
|  10 | 7BBIF  | 28        | 13        | 15       |
|  11 | 7CBIF  | 28        | 11        | 17       |

**(3)** Wie viele Stunden pro Woche sehen die Klassen der Abteilung AIF ihren Klassenvorstand? Lösen Sie
das Beispiel zuerst mit einem klassischen JOIN in Kombination mit einer Gruppierung. Danach lösen Sie
das Beispiel mit einer Unterabfrage ohne JOIN. Betrachten Sie nur Klassen mit eingetragenem Klassenvorstand.
| #   | KNr   | AnzKvStunden |
| ---:| ----- | ------------:|
|   1 | 3BAIF | 7            |
|   2 | 3CAIF | 2            |
|   3 | 5BAIF | 5            |
|   4 | 5CAIF | 2            |

**(4)** Wie viele Wochenstunden haben die Klassen der Abteilung AIF? Achtung: Es gibt Stunden, in denen
2 Lehrer in der Klasse sind. Pro Tag und Stunde ist jeder Datensatz nur 1x zu zählen. Könnten Sie
das Beispiel auch mit einem JOIN und einer Gruppierung lösen? Begründen Sie, wenn nicht.
Anmerkung, die nichts mit der Abfrage zu tun hat: Durch Stundenverlegungen können unterschiedliche
Werte bei Parallelklassen entstehen.
| #   | KNr   | AnzDatensaetze | AnzStunden |
| ---:| ----- | --------------:| ----------:|
|   1 | 2AAIF | 24             | 22         |
|   2 | 2BAIF | 24             | 22         |
|   3 | 2CAIF | 30             | 22         |
|   4 | 2DAIF | 24             | 22         |
|   5 | 3BAIF | 53             | 38         |
|   6 | 3CAIF | 51             | 36         |
|   7 | 4BAIF | 51             | 36         |
|   8 | 4CAIF | 51             | 36         |
|   9 | 5BAIF | 48             | 34         |
|  10 | 5CAIF | 46             | 33         |
|  11 | 6BAIF | 46             | 33         |
|  12 | 6CAIF | 46             | 33         |

Wie viel Prozent der Stunden verbringen die Schüler in ihrem Stammraum? Für diese Anzahl werden einfach
die Anzahl der Datensätze in der Stundentabelle gezählt.
| #   | KNr   | KStammraum | AnzStundenGesamt | AnzStundenStammraum | ProzentImStammraum |
| ---:| ----- | ---------- | ----------------:| -------------------:| ------------------:|
|   1 | 1AHIF | C5.09      | 59               | 12                  | 20                 |
|   2 | 1BHIF | C5.09      | 62               | 11                  | 18                 |
|   3 | 1CHIF | C5.10      | 65               | 19                  | 29                 |
|   4 | 1DHIF | C5.10      | 59               | 14                  | 24                 |
|   5 | 1EHIF | C4.14      | 62               | 9                   | 15                 |
|   6 | 1FHIF | C4.14      | 61               | 8                   | 13                 |
|   7 | 2AHIF | C4.07      | 49               | 23                  | 47                 |
|   8 | 2BHIF | C4.07      | 47               | 6                   | 13                 |
|   9 | 2CHIF | C4.08      | 56               | 13                  | 23                 |
|  10 | 2DHIF | C4.08      | 59               | 16                  | 27                 |
|  11 | 2EHIF | C4.09      | 52               | 23                  | 44                 |
|  12 | 3AHIF | C4.10      | 61               | 25                  | 41                 |
|  13 | 3BHIF | C4.10      | 54               | 19                  | 35                 |
|  14 | 3CHIF | C4.11      | 53               | 16                  | 30                 |
|  15 | 3EHIF | C4.11      | 47               | 14                  | 30                 |
|  16 | 4AHIF | C3.07      | 70               | 37                  | 53                 |
|  17 | 4BHIF | C3.07      | 74               | 16                  | 22                 |
|  18 | 4CHIF | C3.08      | 72               | 17                  | 24                 |
|  19 | 4EHIF | C3.08      | 73               | 40                  | 55                 |
|  20 | 5AHIF | C3.10      | 60               | 15                  | 25                 |
|  21 | 5BHIF | C3.10      | 68               | 27                  | 40                 |
|  22 | 5CHIF | C3.11      | 61               | 23                  | 38                 |
|  23 | 5EHIF | C3.11      | 59               | 17                  | 29                 |

**(5)** Welche Lehrer verdienen 50% mehr als der Durchschnitt von den Lehrern, die vorher in
die Schule eingetreten sind (Eintrittsjahr < Eintrittsjahr des Lehrers)?
| #   | LNr | LName            | LVorname   | LGehalt | LEintrittsjahr | AvgGehaltAeltere |
| ---:| --- | ---------------- | ---------- | -------:| --------------:| ----------------:|
|   1 | MEA | Metz             | Andreas    | 5570    | 1984           | 3100.34          |
|   2 | HL  | Hiesel           | Robert     | 5285    | 1986           | 3086.34          |
|   3 | STJ | Stanek-Schleifer | Julia      | 5355    | 1987           | 3071.70          |
|   4 | HOH | Hohenbüchler     | Heidemarie | 4831    | 1989           | 3060.28          |
|   5 | HAI | Haiker           | Andreas    | 4845    | 1991           | 3037.45          |
|   6 | MAH | Mahler           | Heinrich   | 4746    | 1991           | 3037.45          |
|   7 | ZUM | Zumpf            | Harald     | 4764    | 1993           | 3007.81          |
|   8 | DP  | Divjak           | Peter      | 4390    | 1996           | 2896.73          |
|   9 | FOJ | Forster          | Johanna    | 4472    | 1996           | 2896.73          |
|  10 | JB  | Jagersberger     | Herbert    | 4354    | 1996           | 2896.73          |
|  11 | HOM | Hörzinger        | Michael    | 4329    | 1997           | 2838.04          |

**(6)** Welche Schüler haben im Gegenstand POS1 schlechtere Noten als der Durchschnitt der Prüfungen
bei diesem Prüfer in POS1?
| #   | SNr  | SZuname   | SVorname  | SKlasse | PPruefer | PNote | PGegenstand | PrueferMittel |
| ---:| ----:| --------- | --------- | ------- | -------- | -----:| ----------- | -------------:|
|   1 | 1392 | Hyatt     | Vivian    | 5AKIF   | BAM      | 4     | POS1        | 3.75          |
|   2 | 1455 | DuBuque   | Ramiro    | 5BAIF   | BAM      | 4     | POS1        | 3.75          |
|   3 | 1421 | Mertz     | Florence  | 5CAIF   | BAM      | 5     | POS1        | 3.75          |
|   4 | 1435 | Stark     | Morris    | 5BAIF   | BAM      | 5     | POS1        | 3.75          |
|   5 | 1726 | Rath      | Jasmine   | 5BBIF   | CHA      | 4     | POS1        | 3             |
|   6 | 2083 | Lang      | Roger     | 4CHIF   | GRJ      | 4     | POS1        | 3             |
|   7 | 1813 | Krajcik   | Iris      | 3ACIF   | GT       | 4     | POS1        | 2.33          |
|   8 | 1707 | Kris      | Betsy     | 7ACIF   | PS       | 3     | POS1        | 2.8           |
|   9 | 1690 | Hilll     | Greg      | 7ACIF   | PS       | 4     | POS1        | 2.8           |
|  10 | 1713 | Kling     | Alexandra | 7ACIF   | PS       | 4     | POS1        | 2.8           |
|  11 | 1656 | Reichert  | Judy      | 7CBIF   | UK       | 4     | POS1        | 3.17          |
|  12 | 1672 | Beer      | Joel      | 7BBIF   | UK       | 4     | POS1        | 3.17          |
|  13 | 1674 | Koelpin   | Tina      | 7BBIF   | UK       | 5     | POS1        | 3.17          |
|  14 | 1581 | Jenkins   | Leland    | 3BHIF   | WK       | 4     | POS1        | 3             |
|  15 | 1562 | Hackett   | Grant     | 3BHIF   | WK       | 4     | POS1        | 3             |
|  16 | 1567 | Brown     | Fredrick  | 3BHIF   | WK       | 4     | POS1        | 3             |
|  17 | 1605 | O'Reilly  | Dominick  | 3CHIF   | WK       | 4     | POS1        | 3             |
|  18 | 1588 | Halvorson | Herman    | 3CHIF   | WK       | 5     | POS1        | 3             |

**(7)** Verallgemeinern Sie das vorige Beispiel auf beliebige Fächer: Welche Schüler der 1AHIF 
haben schlechtere Noten als der Prüfer im Mittel für diesen Gegenstand vergibt?
| #   | SNr  | SZuname   | SVorname | SKlasse | PPruefer | PNote | PGegenstand | PrueferMittel |
| ---:| ----:| --------- | -------- | ------- | -------- | -----:| ----------- | -------------:|
|   1 | 1360 | Shields   | Vincent  | 1AHIF   | FRF      | 3     | BWM1        | 2.6           |
|   2 | 1349 | O'Connell | Johnny   | 1AHIF   | FRF      | 4     | BWM1        | 2.6           |
|   3 | 1351 | Monahan   | Barbara  | 1AHIF   | FRF      | 4     | BWM2        | 2.6           |
|   4 | 1362 | Hane      | Lionel   | 1AHIF   | FRF      | 4     | BWM2        | 2.6           |
|   5 | 1360 | Shields   | Vincent  | 1AHIF   | LC       | 3     | POS1z       | 2             |
|   6 | 1351 | Monahan   | Barbara  | 1AHIF   | NIJ      | 4     | SOPK        | 2.67          |

**(8)** Geben Sie die letzte Stunde der 3BAIF für jeden Wochentag aus. Beachten Sie, dass
auch mehrere Datensätze für die letzte Stunde geliefert werden können (wenn 2 Lehrer dort unterrichten).
| #   | StKlasse | StTag | StStunde | StGegenstand | StLehrer |
| ---:| -------- | -----:| --------:| ------------ | -------- |
|   1 | 3BAIF    | 1     | 14       | DBI1         | MIP      |
|   2 | 3BAIF    | 1     | 14       | DBI1         | WES      |
|   3 | 3BAIF    | 2     | 16       | NVS1         | HB       |
|   4 | 3BAIF    | 2     | 16       | NVS1         | OM       |
|   5 | 3BAIF    | 3     | 14       | COPR         | SCM      |
|   6 | 3BAIF    | 3     | 14       | COPR         | HB       |
|   7 | 3BAIF    | 4     | 16       | TINF_1       | EN       |
|   8 | 3BAIF    | 5     | 9        | AM           | SW       |
