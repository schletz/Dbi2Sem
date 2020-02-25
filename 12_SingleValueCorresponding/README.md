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
|   1 | 1AHIF  | 32        | 26        | 6        |
|   2 | 1BHIF  | 32        | 32        | 0        |
|   3 | 1CHIF  | 32        | 32        | 0        |
|   4 | 1DHIF  | 32        | 32        | 0        |
|   5 | 1EHIF  | 32        | 28        | 4        |
|   6 | 1FHIF  | 32        | 32        | 0        |
|   7 | 2AHIF  | 24        | 20        | 4        |
|   8 | 2BHIF  | 27        | 27        | 0        |
|   9 | 2CHIF  | 24        | 24        | 0        |
|  10 | 2DHIF  | 27        | 27        | 0        |
|  11 | 2EHIF  | 27        | 23        | 4        |
|  12 | 3AHIF  | 25        | 21        | 4        |
|  13 | 3BHIF  | 25        | 25        | 0        |
|  14 | 3CHIF  | 22        | 22        | 0        |
|  15 | 3EHIF  | 27        | 21        | 6        |
|  16 | 4AHIF  | 26        | 18        | 8        |
|  17 | 4BHIF  | 25        | 25        | 0        |
|  18 | 4CHIF  | 29        | 29        | 0        |
|  19 | 4EHIF  | 28        | 23        | 5        |
|  20 | 5AHIF  | 28        | 20        | 8        |
|  21 | 5BHIF  | 27        | 27        | 0        |
|  22 | 5CHIF  | 27        | 27        | 0        |
|  23 | 5EHIF  | 26        | 18        | 8        |

**(2)** In welchen Klassen gibt es mehr als doppelt so viel weibliche wie männliche Schüler?

| #   | Klasse | AnzGesamt | AnzMaennl | AnzWeibl |
| ---:| ------ | ---------:| ---------:| --------:|
|   1 | 1CVIF  | 28        | 9         | 19       |
|   2 | 2BHWIT | 24        | 6         | 18       |
|   3 | 2CHWIT | 25        | 6         | 19       |
|   4 | 3ACIF  | 24        | 7         | 17       |
|   5 | 5AKIF  | 23        | 7         | 16       |

**(3)** Wie viele Stunden pro Woche sehen die Klassen der Abteilung AIF ihren Klassenvorstand? Lösen Sie
das Beispiel zuerst mit einem klassischen JOIN in Kombination mit einer Gruppierung. Danach lösen Sie
das Beispiel mit einer Unterabfrage ohne JOIN. Betrachten Sie nur Klassen mit eingetragenem Klassenvorstand.

| #   | KNr   | AnzKvStunden |
| ---:| ----- | ------------:|
|   1 | 2AAIF | 6            |
|   2 | 2BAIF | 0            |
|   3 | 2CAIF | 6            |
|   4 | 3BAIF | 2            |
|   5 | 3CAIF | 2            |
|   6 | 4BAIF | 2            |
|   7 | 4CAIF | 2            |
|   8 | 5BAIF | 8            |
|   9 | 5CAIF | 2            |
|  10 | 6BAIF | 8            |
|  11 | 6CAIF | 2            |

**(4)** Wie viele Wochenstunden haben die Klassen der Abteilung AIF?

| #   | KNr   | AnzStunden |
| ---:| ----- | ----------:|
|   1 | 2AAIF | 24         |
|   2 | 2BAIF | 24         |
|   3 | 2CAIF | 30         |
|   4 | 2DAIF | 24         |
|   5 | 3BAIF | 53         |
|   6 | 3CAIF | 51         |
|   7 | 4BAIF | 51         |
|   8 | 4CAIF | 51         |
|   9 | 5BAIF | 48         |
|  10 | 5CAIF | 46         |
|  11 | 6BAIF | 46         |
|  12 | 6CAIF | 46         |

**(5)** Wie viel Prozent der Stunden verbringen die Schüler der Abteilung KKU (Kolleg für Design) in ihrem
Stammraum? Für diese Anzahl werden einfach die Anzahl der Datensätze in der Stundentabelle gezählt.

| #   | KNr    | KStammraum | AnzStundenGesamt | AnzStundenStammraum | ProzentImStammraum |
| ---:| ------ | ---------- | ----------------:| -------------------:| ------------------:|
|   1 | 3AKKUI | A2.05      | 66               | 52                  | 79                 |
|   2 | 4AKKUI | A2.05      | 64               | 52                  | 81                 |
|   3 | 5AKKUI | A2.04      | 57               | 45                  | 79                 |
|   4 | 6AKKUI | A2.04      | 56               | 44                  | 79                 |

**(6)** Welche Lehrer verdienen 50% mehr als der Durchschnitt von den Lehrern, die nachher in
die Schule eingetreten sind (Eintrittsjahr > Eintrittsjahr des Lehrers)?

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

**(7)** Welche Schüler haben im Gegenstand POS1 schlechtere Noten als der Durchschnitt der Prüfungen
bei diesem Prüfer in POS1?

| #   | SNr  | SZuname   | SVorname | SKlasse | PPruefer | PNote | PGegenstand | PrueferMittel |
| ---:| ----:| --------- | -------- | ------- | -------- | -----:| ----------- | -------------:|
|   1 | 1155 | Pouros    | Dixie    | 5BAIF   | BAM      | 5     | POS1        | 3             |
|   2 | 2371 | Thompson  | Sheri    | 5BBIF   | CHA      | 3     | POS1        | 2.5           |
|   3 | 3334 | Kerluke   | Maureen  | 5AHIF   | GRJ      | 4     | POS1        | 3.33          |
|   4 | 2372 | Turner    | Amos     | 5BBIF   | GT       | 5     | POS1        | 3             |
|   5 | 2406 | Lehner    | Ramon    | 7ACIF   | PS       | 4     | POS1        | 3.25          |
|   6 | 2457 | Schaefer  | Ross     | 7CBIF   | PS       | 5     | POS1        | 3.25          |
|   7 | 2997 | Kozey     | Mario    | 2AHIF   | SCG      | 4     | POS1        | 3.6           |
|   8 | 2998 | Nader     | Noel     | 2AHIF   | SCG      | 5     | POS1        | 3.6           |
|   9 | 2999 | Turcotte  | Leo      | 2AHIF   | SCG      | 5     | POS1        | 3.6           |
|  10 | 2393 | Osinski   | Julie    | 7ACIF   | SE       | 4     | POS1        | 3.25          |
|  11 | 2350 | Aufderhar | Steven   | 5ACIF   | SE       | 4     | POS1        | 3.25          |
|  12 | 2657 | Rodriguez | Bethany  | 5EHIF   | SK       | 3     | POS1        | 2.6           |
|  13 | 2660 | Ryan      | Dominic  | 5EHIF   | SK       | 3     | POS1        | 2.6           |
|  14 | 2925 | Mante     | Julian   | 1EHIF   | SK       | 4     | POS1        | 2.6           |
|  15 | 3297 | Sawayn    | Cedric   | 4EHIF   | UK       | 5     | POS1        | 4.2           |
|  16 | 2369 | Douglas   | Madeline | 5BBIF   | UK       | 5     | POS1        | 4.2           |
|  17 | 2427 | Homenick  | Darla    | 7BBIF   | UK       | 5     | POS1        | 4.2           |
|  18 | 2438 | Beahan    | Josh     | 7BBIF   | UK       | 5     | POS1        | 4.2           |
|  19 | 2440 | Bauch     | Jeremiah | 7BBIF   | UK       | 5     | POS1        | 4.2           |
|  20 | 1108 | Bins      | Jan      | 3AKIF   | ZUM      | 2     | POS1        | 1.83          |
|  21 | 1115 | Schmitt   | Mike     | 3AKIF   | ZUM      | 3     | POS1        | 1.83          |
|  22 | 3072 | Ankunding | Wilbur   | 2CHIF   | ZUM      | 3     | POS1        | 1.83          |

**(8)** Verallgemeinern Sie das vorige Beispiel auf beliebige Fächer: Welche Schüler der 1AHIF 
haben schlechtere Noten als der Prüfer im Mittel für diesen Gegenstand vergibt?

| #   | SNr  | SZuname   | SVorname | SKlasse | PPruefer | PNote | PGegenstand | PrueferMittel |
| ---:| ----:| --------- | -------- | ------- | -------- | -----:| ----------- | -------------:|
|   1 | 2808 | Fahey     | Mark     | 1AHIF   | FRF      | 4     | BWM1        | 3.33          |
|   2 | 2794 | Feil      | Sidney   | 1AHIF   | HOM      | 3     | AMx         | 2             |
|   3 | 2788 | Rodriguez | Rosalie  | 1AHIF   | HOM      | 4     | AMx         | 2             |

**(9)** Geben Sie die letzte Stunde der 3BAIF für jeden Wochentag aus. Beachten Sie, dass
auch mehrere Datensätze für die letzte Stunde geliefert werden können (wenn 2 Lehrer dort unterrichten).

| #   | StKlasse | StTag | StStunde | StGegenstand | StLehrer |
| ---:| -------- | -----:| --------:| ------------ | -------- |
|   1 | 3BAIF    | 1     | 14       | DBI1         | MIP      |
|   2 | 3BAIF    | 1     | 14       | DBI1         | WES      |
|   3 | 3BAIF    | 2     | 16       | NVS1         | OM       |
|   4 | 3BAIF    | 2     | 16       | NVS1         | HB       |
|   5 | 3BAIF    | 3     | 14       | COPR         | SCM      |
|   6 | 3BAIF    | 3     | 14       | COPR         | HB       |
|   7 | 3BAIF    | 4     | 16       | TINF_1       | EN       |
|   8 | 3BAIF    | 5     | 9        | AM           | SW       |
