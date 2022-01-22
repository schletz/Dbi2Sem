# Docker Image als Ersatz für die VM

Die neuste Express Edition (XE) von Oracle gibt es auch als Docker Container. Lade dafür
Docker für dein Betriebssystem von [docs.docker.com](https://docs.docker.com/get-docker/).

Die Installation von Docker Desktop und das Laden des Containers ist als Video
verfügbar: https://youtu.be/eWsscU7ivFI

Nach der erfolgreichen Installation wird der Container für Oracle 21 XE mittels der folgenden
Befehle in der Windows Konsole geladen und ausgeführt. Der Container hat rund 3.5 GB.

```text
docker pull gvenzl/oracle-xe:21-full
docker run -d -p 1521:1521 -e ORACLE_PASSWORD=oracle -v oracle-volume:/opt/oracle/XE21CFULL/oradata --name oracle21c gvenzl/oracle-xe:21-full
```
Die Umgebungsvariable *ORACLE_PASSWORD* setzt das Systempasswort. Da es keine Produktionsdatenbank
ist, verwenden wir zur Vereinfachung *oracle*.

## Starten und Stoppen des Containers

Durch *docker run* wird unser Container bereits gestartet. Aber wie verhält es sich nach einem
Neustart von Windows? Docker Desktop startet automatisch mit
Microsoft Windows, der Container wird allerdings nicht automatisch gestartet.
Daher die zwei folgenden Befehle in der Konsole zum Starten bzw. manuellen Stoppen (wenn notwendig)
des Containers wichtig:

```text
docker start oracle21c
docker stop oracle21c
```

Natürlich kann mit Docker Desktop der Container ebenfalls gestartet und beendet werden.

## Ausführen von Programmen im Container

Mit *docker exec -it oracle21c COMMAND* können Befehle direkt im Container ausgeführt werden.
Die Option *-i* bedeutet eine interaktive Ausführung. *-t* öffnet ein Terminal, sodass nicht CR+LF
von Windows gesendet wird (Linux verwendet nur CR).

### SQL*Plus 

SQL*Plus ist ein Kommandozeilentool, welches direkt SQL Befehle absetzen kann. Wollen wir als
System User direkt Befehle in der pluggable database absetzen, können wir
mittels *docker exec* das Dienstprogramm *sqlplus* starten. Das Passwort ist *oracle* und wurde
im *docker run* Befehl weiter oben als Umgebungsvariable *ORACLE_PASSWORD* gesetzt.

```text
docker exec -it oracle21c sqlplus system/oracle@//localhost/XEPDB1
```

Wollen wir *systemweite Änderungen* machen, gibt es noch den User *SYS*. Hier können Konfigurationen,
die das ganze System betreffen, gelesen und gesetzt werden:

```text
docker exec -it oracle21c sqlplus SYS AS SYSDBA
```

Mit dem Befehl *quit* kann der SQL*Plus Prompt verlassen werden.

### Shell (bash)

Wir können auch eine Shell öffnen und Befehle in Linux absetzen:

```text
docker exec -it oracle21c /bin/bash
```

Mit *exit* kann die Shell verlassen und zu Windows zurückgekehrt werden.
