# Aufgabenstellung zu Unterabfragen

## Datenbank und Schema

Eine Datenbank soll das Backend einer kleinen Pizzaria mit Lieferservice simulieren.
Diese Datenbank hat folgendes Schema:

![](datenmodell.png)

- Die Tabelle *Kunden* speichert alle registrierten Kunden. Die Adresse wird bei jeder Bestellung
  angegeben, deswegen findet sie sich nicht dort wieder.
- Da unterschiedliche Lieferzuschläge verrechnet werden, wurde eine Tabelle *Liefergebiet* erstellt.
  Als Schlüssel dient die Kombination zwischen PLZ und Ort, da eine PLZ auch mehrere Orte umfassen
  kann. Der Lieferzuschlag ist der Preis, der pro Bestellung aufgeschlagen wird.
- Die Bestellung des Kunden ist in der Tabelle *Bestellung* erfasst. Dort wird die Adresse gespeichert,
  an die die Speisen geliefert werden.
- Die verfügbaren Speisen sind in der Taballe *Produkt* erfasst. Sie sind jeweils einer Kategorie
  zugeordnet (Pizza, Fisch, Pasta, ...)  
- Zwischen Bestellung und Produkt stellt die Tabelle *ProduktBestellung* die Auflösung der n:m
  Beziehung dar, da eine Bestellung natürlich mehrere Produkte umfassen kann.

## Generieren der Datenbank

Öffne in Docker Desktop eine Shell des Oracle oder SQL Server Containers. Kopiere danach die
folgenden Befehle aus. Sie laden die .NET 6 SDK und den Generator der Datenbank. Drücke
*Enter*, um die Befehle auszuführen. Am Ende wirst du nach dem Admin Passwort der Datenbank
gefragt. Hast du den Container mit den Standardpasswörtern (*oracle* für Oracle bzw. *SqlServer2019*
für Sql Server 2019) erstellt, musst du nur *Enter* drücken.

```bash
if [ -d "/opt/oracle" ]; then 
    DOWNLOADER="curl -s"
    RUNCMD="export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 && dotnet run -- oracle"
else 
    HOME=/tmp
    DOWNLOADER="wget -q -O /dev/stdout"
    RUNCMD="export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0 && dotnet run -- sqlserver"
fi

cd $HOME
$DOWNLOADER https://raw.githubusercontent.com/schletz/Dbi2Sem/master/dotnet_install.sh > dotnet_install.sh
chmod a+x dotnet_install.sh
. ./dotnet_install.sh

mkdir -p $HOME/lieferservice
cd $HOME/lieferservice
for srcfile in Lieferservice.csproj Bestellung.cs Kategorie.cs Kunde.cs Liefergebiet.cs LieferserviceContext.cs MultiDbContext.cs Produkt.cs ProduktBestellung.cs Program.cs 
do
    $DOWNLOADER https://raw.githubusercontent.com/schletz/Dbi2Sem/master/lieferservice/$srcfile > $srcfile
done
eval $RUNCMD
```

## Bewertung und Abgabe

Jede korrekt gelöste Aufgabe bekommt 1 Punkt. Eine Aufgabe gilt als korrekt gelöst, wenn

- Die ausgegebenen Datensätze mit der Musterlösung übereinstimmen. Formatierung und Sortierung
  müssen allerdings nicht beachtet werden, Spaltennamen jedoch schon.
- Bei Zahlen kann es durch die Maschinengenauigkeit zu leichten Abweichungen
  in der letzten Kommastelle kommen (statt 0.1 wird 0.099999999 ausgegeben). Das ist in Ordnung.  
- Die Abfrage allgemeingültig ist, also keine fix eingetragenen Werte hat oder Sonderfälle, die
  nur in diesem Datenbestand zutreffen, verwendet.

Verwenden Sie das untenstehende Muster für Ihre SQL Datei und schreiben Sie Ihren Namen
hinein. Schreiben Sie Ihre Lösung unter den entsprechenden Kommentar mit der Aufgabennummer.
Geben Sie diese Datei in Teams bis 7. Mai 2020 um 12:00 ab.

Würde die Aufgabenstellung separat benotet werden, ergibt sich - zu Ihrer Selbteinschätzung -
folgende Skala: 12 Punkte: 1, 11 - 10 Punkte: 2, 9 - 8 Punkte: 3, 7 Punkte: 4

## Aufgaben

**(1)** Für welche Liefergebiete wird der meiste Lieferzuschlag verrechnet?

| Plz  | Ort  | Lieferzuschlag |
| ---- | ---- | -------------- |
| 1180 | Wien | 8.0            |
| 1170 | Wien | 8.0            |
| 1160 | Wien | 8.0            |

**(2)** Geben Sie die teuersten Produkte in der Kategorie 2 (Pasti) aus. Beachten Sie, dass
dies auch mehr Produkte sein können.

| ProduktId | Name           | KategorieId | Preis |
| --------- | -------------- | ----------- | ----- |
| 5         | All Arabiata   | 2           | 7.5   |
| 6         | Alla Bolognese | 2           | 7.5   |

**(3)** Welche Produkte aus der Kategorie 2 sind teurer als das teuerste Produkt der
Kategorie 1. Geben Sie den Preis des teuersten Produktes der Kategorie 1 in der Spalte
*MaxKategorie1* aus.

| ProduktId | Name           | KategorieId | Preis | MaxKategorie1 |
| --------- | -------------- | ----------- | ----- | ------------- |
| 5         | All Arabiata   | 2           | 7.5   | 7.0           |
| 6         | Alla Bolognese | 2           | 7.5   | 7.0           |

**(4)** Welche Bestellungen beinhalten Produkte der Kategorie Pizza (Kategorie-ID 1)?

| BestellungId | Adresse              | LiefergebietPlz | LiefergebietOrt | KundeId | Bestellzeit         |
| ------------ | -------------------- | --------------- | --------------- | ------- | ------------------- |
| 5            | 498 Christina Bypass | 1160            | Wien            | 3       | 2020-05-04 07:41:29 |
| 6            | 859 Lucius Forges    | 1160            | Wien            | 3       | 2020-05-01 18:17:26 |
| 8            | 332 Bryce Circle     | 1050            | Wien            | 5       | 2020-05-03 22:27:50 |
| 11           | 334 Ritchie Haven    | 1170            | Wien            | 6       | 2020-05-05 00:22:08 |
| 13           | 586 Parisian Manors  | 1050            | Wien            | 7       | 2020-05-02 22:23:38 |
| 14           | 93592 Juana Village  | 1060            | Wien            | 9       | 2020-05-03 02:59:55 |
| 15           | 0364 Della Harbor    | 1040            | Wien            | 9       | 2020-05-01 23:27:30 |

**(5)** Welche Kunden gaben nie eine Bestellung auf?

| KundeId | Vorname | Zuname  | Email         |
| ------- | ------- | ------- | ------------- |
| 4       | Jonas   | Fischer | jonas@mail.at |
| 8       | Oskar   | Becker  | oskar@mail.at |

**(6)** Welche Kunden bestellten eine Speise aus der Kategorie 3 (Pesce)

| KundeId | Vorname    | Zuname  | Email              |
| ------- | ---------- | ------- | ------------------ |
| 1       | Lukas      | Müller  | lukas@mail.at      |
| 2       | Konstantin | Schmidt | konstantin@mail.at |
| 5       | Elias      | Weber   | elias@mail.at      |
| 7       | David      | Wagner  | david@mail.at      |
| 9       | Philipp    | Schulz  | philipp@mail.at    |

**(7)** Welche Kunden bestellten eine Speise aus der Kategorie 1 (Pizza) und 3 (Pesce)

| KundeId | Vorname | Zuname | Email           |
| ------- | ------- | ------ | --------------- |
| 5       | Elias   | Weber  | elias@mail.at   |
| 7       | David   | Wagner | david@mail.at   |
| 9       | Philipp | Schulz | philipp@mail.at |

**(8)** Welche Kunden bestellten nie Speisen aus der Kategorie 2 (Pasti)? Schließen Sie
alle Kunden aus, die nie etwas bestellt haben.

| KundeId | Vorname    | Zuname  | Email              |
| ------- | ---------- | ------- | ------------------ |
| 2       | Konstantin | Schmidt | konstantin@mail.at |

**(9)** Listen Sie zu jeder Bestellung die Anzahl der Produkte pro Kategorie-ID. Die Anzahl berechnet
sich aus der Summe der Menge. Sie können annehmen,
dass als Kategorie-ID nur die Werte 1, 2 oder 3 vorkommt. Geben Sie diese Werte in der Spalte
Kategorie1, Kategorie2 bzw. Kategorie3 aus.

| BestellungId | Adresse                | LiefergebietPlz | LiefergebietOrt | KundeId | Bestellzeit         | MengeKategorie1 | MengeKategorie2 | MengeKategorie3 |
| ------------ | ---------------------- | --------------- | --------------- | ------- | ------------------- | --------------- | --------------- | --------------- |
| 1            | 26071 Lesch Extensions | 1040            | Wien            | 1       | 2020-05-05 23:35:25 |                 | 4               |                 |
| 2            | 93578 Marlen Squares   | 1050            | Wien            | 1       | 2020-05-01 16:23:38 |                 | 1               | 2               |
| 3            | 08118 Goyette Park     | 1180            | Wien            | 1       | 2020-05-01 04:53:46 |                 | 3               | 1               |
| 4            | 55660 Alysa Path       | 1180            | Wien            | 2       | 2020-05-02 00:24:22 |                 |                 | 3               |
| 5            | 498 Christina Bypass   | 1160            | Wien            | 3       | 2020-05-04 07:41:29 | 3               |                 |                 |
| 6            | 859 Lucius Forges      | 1160            | Wien            | 3       | 2020-05-01 18:17:26 | 5               | 2               |                 |
| 7            | 20314 Ronny Crescent   | 1180            | Wien            | 5       | 2020-05-04 18:53:43 |                 | 3               |                 |
| 8            | 332 Bryce Circle       | 1050            | Wien            | 5       | 2020-05-03 22:27:50 | 2               |                 |                 |
| 9            | 814 Ruth Terrace       | 1160            | Wien            | 5       | 2020-05-02 02:16:43 |                 | 3               | 2               |
| 10           | 465 Cartwright Canyon  | 1050            | Wien            | 6       | 2020-05-03 21:27:04 |                 | 5               |                 |
| 11           | 334 Ritchie Haven      | 1170            | Wien            | 6       | 2020-05-05 00:22:08 | 2               | 1               |                 |
| 12           | 627 Josianne Ways      | 1170            | Wien            | 7       | 2020-05-04 05:13:11 |                 |                 | 1               |
| 13           | 586 Parisian Manors    | 1050            | Wien            | 7       | 2020-05-02 22:23:38 | 1               | 1               |                 |
| 14           | 93592 Juana Village    | 1060            | Wien            | 9       | 2020-05-03 02:59:55 | 4               | 1               |                 |
| 15           | 0364 Della Harbor      | 1040            | Wien            | 9       | 2020-05-01 23:27:30 | 2               |                 | 3               |

**(10)** Erstellen Sie eine View *vBestellungen*, die pro Bestellung die Kundendaten und den Gesamtbetrag
ausgibt. Der Gesamtbetrag berechnet sich aus *Menge x Preis + Lieferzuschlag*
Beachten Sie folgende Hinweise:

- Der Lieferzuschlag wird pro Bestellung nur 1x aufgeschlagen. Daher müssen Sie zuerst den
  Wert der Bestellung mit Menge x Preis in einer Unterabfrage ermitteln.
- Die Tabelle Liefergebiet hat einen zusammengesetzten Schlüssel aus PLZ und Ort, den Sie
  mit der Tabelle Bestellung verknüpfen müssen.

| KundeId | Vorname    | Zuname    | Email              | BestellungId | Bestellzeit         | Adresse                | Lieferzuschlag | Betrag |
| ------- | ---------- | --------- | ------------------ | ------------ | ------------------- | ---------------------- | -------------- | ------ |
| 1       | Lukas      | Müller    | lukas@mail.at      | 1            | 2020-05-05 23:35:25 | 26071 Lesch Extensions | 5.0            | 33     |
| 1       | Lukas      | Müller    | lukas@mail.at      | 2            | 2020-05-01 16:23:38 | 93578 Marlen Squares   | 5.0            | 31.5   |
| 1       | Lukas      | Müller    | lukas@mail.at      | 3            | 2020-05-01 04:53:46 | 08118 Goyette Park     | 8.0            | 40     |
| 2       | Konstantin | Schmidt   | konstantin@mail.at | 4            | 2020-05-02 00:24:22 | 55660 Alysa Path       | 8.0            | 36.5   |
| 3       | Ben        | Schneider | ben@mail.at        | 5            | 2020-05-04 07:41:29 | 498 Christina Bypass   | 8.0            | 23     |
| 3       | Ben        | Schneider | ben@mail.at        | 6            | 2020-05-01 18:17:26 | 859 Lucius Forges      | 8.0            | 56     |
| 5       | Elias      | Weber     | elias@mail.at      | 7            | 2020-05-04 18:53:43 | 20314 Ronny Crescent   | 8.0            | 30.5   |
| 5       | Elias      | Weber     | elias@mail.at      | 8            | 2020-05-03 22:27:50 | 332 Bryce Circle       | 5.0            | 19     |
| 5       | Elias      | Weber     | elias@mail.at      | 9            | 2020-05-02 02:16:43 | 814 Ruth Terrace       | 8.0            | 53.5   |
| 6       | Niklas     | Meyer     | niklas@mail.at     | 10           | 2020-05-03 21:27:04 | 465 Cartwright Canyon  | 5.0            | 42.5   |
| 6       | Niklas     | Meyer     | niklas@mail.at     | 11           | 2020-05-05 00:22:08 | 334 Ritchie Haven      | 8.0            | 28.5   |
| 7       | David      | Wagner    | david@mail.at      | 12           | 2020-05-04 05:13:11 | 627 Josianne Ways      | 8.0            | 19.5   |
| 7       | David      | Wagner    | david@mail.at      | 13           | 2020-05-02 22:23:38 | 586 Parisian Manors    | 5.0            | 18.5   |
| 9       | Philipp    | Schulz    | philipp@mail.at    | 14           | 2020-05-03 02:59:55 | 93592 Juana Village    | 5.0            | 38.5   |
| 9       | Philipp    | Schulz    | philipp@mail.at    | 15           | 2020-05-01 23:27:30 | 0364 Della Harbor      | 5.0            | 45.5   |

**(11)** Erstellen Sie eine View *vKundenumsatz*, die pro Kunde den Gesamtumsatz berechnet.
Sie können entweder Ihre View *vBestellungen* verwenden oder die Abfrage neu aufbauen.

| KundeId | Vorname    | Zuname    | Gesamtumsatz |
| ------- | ---------- | --------- | ------------ |
| 1       | Lukas      | Müller    | 104.5        |
| 2       | Konstantin | Schmidt   | 36.5         |
| 3       | Ben        | Schneider | 79           |
| 5       | Elias      | Weber     | 103          |
| 6       | Niklas     | Meyer     | 71           |
| 7       | David      | Wagner    | 38           |
| 9       | Philipp    | Schulz    | 84           |

**(12)** Listen Sie die umsatzstärksten Kunden auf. Verwenden Sie zur Beantwortung die View
*vKundenumsatz*. Beachten Sie, dass auch mehrere Kunden diesen Umsatz erreichen können.

| KundeId | Vorname | Zuname | Gesamtumsatz |
| ------- | ------- | ------ | ------------ |
| 1       | Lukas   | Müller | 104.5        |

## Vorlage für die SQL Datei

```sql
ÜBUNG ZU SQL UNTERABFRAGEN
ZUNAME VORNAME
4BAIF, 6. und 7. Mai 2020

Aufgabe 1

Aufgabe 2

Aufgabe 3

Aufgabe 4

Aufgabe 5

Aufgabe 6

Aufgabe 7

Aufgabe 8

Aufgabe 9

Aufgabe 10

Aufgabe 11

Aufgabe 12
```
