# Vorbereitung der Umgebung für Oracle

## Installation von Docker und laden des Containers

Siehe [Docker Image für Oracle 21 XE](03_Docker/README.md)

## Generierung der Schuldatenbank

Zum Generieren der Schuldatenbank starten Sie den [SchulDbGenerator](../SchulDbGenerator/README.md).
Dieses Programm legt einen Benutzer und die Datenbank in Ihrer Oracle VM an.

## Installation von SQL Developer

SQL Developer ist zwar in der virtuellen Maschine integriert, eine Installation unter ihrem
Hostbetriebssystem erlaubt allerdings ein flüssigeres Arbeiten. Dafür verbindet sich SQL Developer
über TCP (Port 1521) zu Ihrer virtuellen Maschine, die natürlich laufen muss.

Sie können SQL Developer 22 entweder direkt von der
[Downloadseite von Oracle](https://www.oracle.com/database/sqldeveloper/technologies/download/) laden.
Die ZIP Datei muss nur entpackt und sqldeveloper.exe gestartet werden. Für den Download ist
eine registrierung bei Oracle nötig.

Das Programm *SchulDbGenerator* hat im vorigen Schritt einen Benutzer und ein Kennwort in Ihrer VM
angelegt. Diese Daten werden am Ende angezeigt, denn Sie benötigen Sie nun. Zum Anlegen einer
Verbindung klicken Sie wieder auf das grüne Plus in der Palette Connections und richten die
Verbindung wie folgt ein:

- Verbindungsname: frei wählbar (s. B. *SchulDbConn*)
- Benutzername: *Schule* (wie im SchulDbGenerator angezeigt)
- Kennwort: *oracle* (wie im SchulDbGenerator angezeigt)
- Service-Name (statt SID): *orcl*

Nach dem Klick auf Test und Save steht die neue Verbindung nun in der Palette Connections zur
Verfügung. Ein Klick auf die Verbindung öffnet das Abfragefenster.

![](images/sqlDeveloperConnection.png)

> Wenn Sie sich als System User verbinden wollen, geben Sie als Benutzername *system* und als
> Passwort *oracle* ein.

### Weitere Informationen

- [Verwenden von DBeaver](01_Dbeaver/README.md)
- [Verwenden von JetBrains DataGrip](02_DataGrip/README.md)
