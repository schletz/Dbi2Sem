# Generalisierung

Betrachten Sie die folgenden Datenmodelle:

![](arten20200209.png)

## Variante 1: Eine kleine Bibliothek

In einer Bibliothek können verschiedene Werke ausgeliehen werden. Jedes Werk hat einen Titel, einen
Verlag und einen Autor. Bücher haben zusätzlich noch eine Auflage und ein Erscheinungsjahr. Bei
einer Zeitschrift soll zusätzlich noch ein Erscheinungsdatum gespeichert werden. Eine Disc hat
mehrere Titel mit Namen.

Aus dieser Information können wir die benötigten Attribute schon herausfinden:

| Entity      | Attribute                                                 |
| ----------- | --------------------------------------------------------- |
| Buch        | **ISBN**, Titel, Verlag, Autor, Auflage, Erscheinungsjahr |
| Zeitschrift | **ISBN**, Titel, Verlag, Autor, Erscheinungsdatum         |
| Disc        | **ISBN**, Titel, Verlag, Autor                            |

Nun sehen wir, dass einige Attribute in jeder Tabelle vorkommen. Diese können wir in einem eigenen
Obertyp (*Supertyp*) zusammenfassen. Diesen Vorgang nennt man *Generalisierung*. Es entsteht
eine neue Tabelle *Werk*, die Titel, Verlag und Autor beinhaltet.

| Entity      | Attribute                           |
| ----------- | ----------------------------------- |
| Werk        | **ISBN**, Titel, Verlag, Autor      |
| Buch        | **ISBN**, Auflage, Erscheinungsjahr |
| Zeitschrift | **ISBN**, Erscheinungsdatum         |
| Disc        | **ISBN**                            |

Alle verbliebenen Untertypen (Buch, Zeitschrift, Disc) nennt man *Subtypen*. Zwischen diesen
Typen und dem Werk besteht eine *IS-A* Beziehung. Ein Buch ist ein Werk, eine Zeitschrift ist ein
Werk und eine Disc ist ein Werk. Jeder Untertyp erbt alle Attribute des Obertypen.

### Die Vererbung in der Programmierung

Diese Ideen sind nicht neu. In der objektorientierten Programmierung gibt es das Konzept der
Vererbung, die genau diesen Sachverhalt abbildet. In C# zeigt der Doppelpunkt die Vererbung an.
Die Klasse Buch hat also auch die Properties Isbn, Titel, ... In Java wird dies mit *extends*
gekennzeichnet.

```c#
class Werk
{
    public string Isbn { get; set; }
    public string Titel { get; set; }
    public string Verlag { get; set; }
    public string Autor { get; set; }
}

class Buch : Werk
{
    public int Auflage { get; set; }
    public int Erscheinungsjahr { get; set; }
}

class Zeitschrift : Werk
{
    public DateTime Erscheinungsdatum { get; set; }
}

class Disc : Werk
{

}
```

### Arten der Spezialisierung

- **Disjunkte Spezialisierung:** Eine Entität kann höchstens einem Untertyp gehören. Es kann
also nicht sein, dass ein Wert ein Buch und eine Disc zugleich - also in beiden Tabellen vorhanden -
ist. (disjunkt = durchschnittsfremd)

- **Vollständige Spezialisierung:** Eine Entität muss mindestens zu einem Untertyp gehören. Es
kann also nicht sein, dass wir nur ein Werk ohne Typ (Buch, Zeitschrift oder Disc) haben.

## Variante 2: Eine Leihwagenfirma

Wir stellen uns vor, dass eine Leihwagenfirma 2 Arten von Fahrzeugen anbietet:

| Fahrzeug    | Attribute                                       |
| ----------- | ----------------------------------------------- |
| PKW         | **ID**, Kennzeichen, Kilometerstand, AnzPlaetze |
| Transporter | **ID**, Kennzeichen, Kilometerstand, Nutzlast   |

Wir können hier auch nach Variante 1 vorgehen, einen Supertyp *Fahrzeug* anlegen und 2 Subtypen
(PKW und Transporter) damit verbinden. Dieses Modell geht allerdings einen anderen Weg:
Es speichert in einer einzigen Tabelle (Fahrzeug) *alle Spalten von PKW und Transporter*. Natürlich
müssen die Spalten *AnzPlaetze* und *Nutzlast* nullable sein, da sie nicht überall vorkommen.

Zusätzlich muss noch die Art gespeichert werden. Dies geschieht im Attribut *Kategorie*. In der
Lookup Tabelle sind dann 2 Datensätze (PKW und Transporter) gespeichert.

Dieses Modell hat jedoch die Einschränkung, dass *ein Fahrzeug höchstens eine Kategorie* hat. Es
kann kein Fahrzeug, welches ein PKW und ein Transporter zugleich ist, gespeichert werden.

## Variante 3: Unsere Schule

Auch in Variante 3 sind gleiche Attribute zwischen Schüler und Lehrer zu finden (Vorname, Nachname).
Allerdings sind hier die Tabellen vollkommen getrennt gespeichert. Auch das ist eine Möglichkeit,
vor allem wenn wir keinen generalisierten Obertyp (Person) im Modell sinnvoll verwenden können. Das
ist hier der Fall, denn Schüler und Lehrer sind völlig getrennte Rollen und nicht austauschbar.

## Auflösung mittels Rollup und Rolldown

Das Problem mit Variante 1 ist, dass eine Datenbank - im Gegensatz zu objektorientierten
Programmiersprachen wie C# oder Java - bei *CREATE TABLE* keine spezielle Syntax für diesen
Sachverhalt kennt. Die CREATE TABLE Anweisungen sind die Gleichen wie für eine normale
Fremdschlüsselbeziehung:

```sql
CREATE TABLE Werk (
    ISBN   CHAR(13) PRIMARY KEY,
    ...
);

CREATE TABLE Buch (
    ISBN   CHAR(13) PRIMARY KEY REFERENCES Werk(ISBN)
    ...
)
```

Um ein Buch anzulegen, muss zuerst das Werk und in einer 2. INSERT Anweisung das Buch angelegt
werden. Außerdem können wir der Datenbank nicht sagen, dass es sich um eine disjunkte
Spezialisierung handelt, also ein Werk nur ein Buch, eine Zeitschrift oder eine Disc haben kann.

Auch in Zusammenarbeit mit einem OR Mapper, der die Datenbank in C# oder Javaklassen abbildet,
gibt es mit der Generalisierung Probleme. EF Core würde - wenn Sie Vererbung verwenden - alle
Attribute in einer einzigen Tabelle speichern:

> At the moment, EF Core only supports the table-per-hierarchy (TPH) pattern. TPH uses a single table
> to store the data for all types in the hierarchy, and a discriminator column is used to identify
> which type each row represents.
> <sup>https://docs.microsoft.com/en-us/ef/core/modeling/inheritance</sup>

### Rolldown: Hinzufügen der Attribute zu den Subtypen, Löschen des Supertypen

Dieser Ansatz entspricht eigentlich unserem Ausgangspunkt. Beachten Sie hier die Beziehung
zwischen Autor und den einzelnen Subtypen: Jeder Subtyp hat nun einen Fremdschlüssel Autor,
da die Autortabelle an den Subtyp gebunden war.

Der Track verweist nach wie vor auf die Tabelle *Disc*.

| Entity      | Attribute                                                 |
| ----------- | --------------------------------------------------------- |
| Autor       | **ID**, Name, Vorname                                     |
| Buch        | **ISBN**, Titel, Verlag, Autor, Auflage, Erscheinungsjahr |
| Zeitschrift | **ISBN**, Titel, Verlag, Autor, Erscheinungsdatum         |
| Disc        | **ISBN**, Titel, Verlag, Autor                            |
| Track       | **Disc**, **Nr**, Name, Startposition, Dauer              |

### Rollup: Hinzufügen der Attribute zum Supertyp

Hier bleibt nur die Tabelle Werk. Die einzelnen Spalten der Subtypen sind nun integriert und dürfen
natürlich NULL Werte enthalten. Beachten Sie die Tabelle Track. Sie ist nun an das Werk gebunden
und hat sinnvollerweise nur Einträge für Daten der Kategorie Disc.

| Entity    | Attribute                                                                               |
| --------- | --------------------------------------------------------------------------------------- |
| Autor     | **ID**, Name, Vorname                                                                   |
| Kategorie | **ID**, Name                                                                            |
| Werk      | **ISBN**, Kategorie, Titel, Verlag, Autor, Auflage, Erscheinungsjahr, Erscheinungsdatum |
| Track     | **Werk**, **Nr**, Name, Startposition, Dauer                                            |

### Was soll ich für die Auflösung wählen

Verwenden Sie als Kriterium die Tabellen, die sie im Modell mit dem Sub- oder Supertyp verbinden.
Haben Sie die Situation, dass der Supertyp (hier das Werk) viele Beziehungen zu anderen Tabellen hat,
werden Sie dies mit Rollup leichter umsetzen können. Verwenden Sie allerdings oft den spezifischen
Subtyp in Beziehung mit anderen Tabellen, so wird der Rolldown Ansatz reibungsloser funktionieren.

## Übung

**(1)** Ein Fußballverein gibt ihnen folgende Informationen:

- Ein Mitglied des Vereines wird mit Vorname, Nachname und Geburtsdatum erfasst.
- Das Mitglied kann entweder ein Spieler oder ein Trainer sein. Der Spieler hat eine bevorzugte
  Position (Mittelfeld, Tormann, ...). Bei einem Trainer wird das Datum der Trainerprüfung zusätzlich
  gespeichert.
- Bei einem Spiel wird die Mannschaft aus mehreren Spielern und einem Trainer zusammengestellt und
  auf die Reise geschickt.

Modellieren Sie zuerst diesen Sachverhalt mit einer generalisierten Mitgliedertabelle. Danach führen
Sie Rollup und Rolldown durch. Was halten Sie für sinnvoller?

**(2)** Eine Immobilienfirma möchte ihre Objekte, die sie anbietet, verwalten. Die Objekte haben
eine Adresse, einen Besitzer (Vor- und Nachname sowie Telefonnummer) und eine Größe in qm.
Es gibt bei den Immobilien Häuser, Eigentumswohnungen, Mietwohnungen und Firmengebäude. Mietwohnungen
haben eine Monatsmiete, alles Andere hat einen Verkaufspreis.

Modellieren Sie diesen Sachverhalt wieder in Form einer Generalisierung. Danach führen Sie Rollup und
Rolldown durch. Was halten Sie für sinnvoller?
